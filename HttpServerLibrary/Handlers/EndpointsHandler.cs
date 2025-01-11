using HttpServerLibrary.Attributes;
using HttpServerLibrary.HttpResponse;
using System.Reflection;

namespace HttpServerLibrary.Handlers
{
    internal sealed class EndpointsHandler : Handler
    {
        private readonly Dictionary<string, List<(HttpMethod method, MethodInfo methodInfo, Type endpointType)>> _routes = new();

        public EndpointsHandler()
        {
            RegisterEndpointsFromAssemblies(new[] { Assembly.GetEntryAssembly() });

        }

        public override void HandleRequest(HttpRequestContext context)
        {
            var request = context.Request;
            var url = request.Url.LocalPath.Trim('/');
            var requestMethod = request.HttpMethod;

            if (_routes.ContainsKey(url))
            {
                var route = _routes[url].FirstOrDefault(r => r.method.ToString().Equals(requestMethod, StringComparison.OrdinalIgnoreCase));
                if(route.methodInfo != null){
                    var endpointInstance = Activator.CreateInstance(route.endpointType) as EndpointBase;
                    
                    if(endpointInstance != null)
                    {
                        endpointInstance.SetContext(context);

                        // вызываем метод
                        var parameters = GetMethodParameters(route.methodInfo, context);
                        var result = route.methodInfo.Invoke(endpointInstance, parameters) as IHttpResponseResult;
                        result?.Execute(context);
                    }
                }
            }
            else if (Successor != null)
            {
                // TODO: добавить Handler 404 ошибки
                Successor.HandleRequest(context);
            }
        }

        private void RegisterEndpointsFromAssemblies(Assembly[] assemblies)
        {
            foreach (Assembly assembly in assemblies)
            {
                var endpointTypes = assembly.GetTypes()
                    .Where(t => typeof(EndpointBase).IsAssignableFrom(t) && !t.IsAbstract);

                foreach (var endpointType in endpointTypes)
                {
                    var methods = endpointType.GetMethods();
                    foreach (var methodInfo in methods)
                    {
                        // TODO: Можно отрефакторить
                        var getAttribute = methodInfo.GetCustomAttribute<GetAttribute>();
                        if (getAttribute != null)
                        {
                            RegisterRoute(getAttribute.Route, HttpMethod.Get, methodInfo, endpointType);
                        }

                        var postAttribute = methodInfo.GetCustomAttribute<PostAttribute>();
                        if (postAttribute != null)
                        {
                            RegisterRoute(postAttribute.Route, HttpMethod.Post, methodInfo, endpointType);
                        }
                    }
                }
            }
        }

        private void RegisterRoute(string route, HttpMethod method, MethodInfo methodInfo, Type endpointType)
        {
            // TODO: добавить проверку на одинаковые роутинги и одинаковые HttpMethod, выкидывать исключение
            // с сообщеинем в консоль и прекратить работу сервера

            if (!_routes.ContainsKey(route))
            {
                _routes[route] = new();
            }

            _routes[route].Add((method, methodInfo, endpointType));
        }

        private object[] GetMethodParameters(MethodInfo method, HttpRequestContext context)
        {
            var parameters = method.GetParameters();
            var values = new object[parameters.Length];

            if (context.Request.HttpMethod.Equals("GET", StringComparison.InvariantCultureIgnoreCase))
            {
                // Извлекаем параметры из строки запроса
                var queryParameters = System.Web.HttpUtility.ParseQueryString(context.Request.Url.Query);
                for (int i = 0; i < parameters.Length; i++)
                {
                    var paramName = parameters[i].Name;
                    var paramType = parameters[i].ParameterType;
                    var value = queryParameters[paramName];
                    values[i] = ConvertValue(value, paramType);
                }
            }
            else if (context.Request.HttpMethod.Equals("POST", StringComparison.InvariantCultureIgnoreCase))
            {
                // Извлекаем параметры из тела запроса
                using var reader = new StreamReader(context.Request.InputStream);
                var body = reader.ReadToEnd();

                if (context.Request.ContentType == "application/x-www-form-urlencoded")
                {
                    var formParameters = System.Web.HttpUtility.ParseQueryString(body);
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var paramName = parameters[i].Name;
                        var paramType = parameters[i].ParameterType;
                        var value = formParameters[paramName];
                        values[i] = ConvertValue(value, paramType);
                    }
                }
                else if (context.Request.ContentType == "application/json")
                {
                    // Парсим JSON в объект
                    var jsonObject = System.Text.Json.JsonSerializer.Deserialize(body, method.GetParameters()[0].ParameterType);
                    return new[] { jsonObject };
                }
            }

            return values;
        }

        private object ConvertValue(string value, Type targetType)
        {
            if (value == null)
            {
                return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
            }

            return Convert.ChangeType(value, targetType);
        }
    }
}
