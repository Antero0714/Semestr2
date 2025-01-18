using HttpServerLibrary;
using HttpServerLibrary.Attributes;
using HttpServerLibrary.HttpResponse;
using System.Data.SqlClient;
using MyHttpServer.Models;
using MyORMLibrary;
using HttpServerLibrary.Configurations;

namespace MyHttpServer.Endpoints
{

    internal class UserEndpoits : EndpointBase
    {
        [Get("Films")]
        public IHttpResponseResult GetUser(string login = null, string password = null)
        {
            string connectionString = AppConfig.Instance.ConnectionString;

            using var sqlConnection = new SqlConnection(connectionString);
            var dbContext = new ORMContext<Film>(sqlConnection);

            // Укажите название таблицы
            var user = dbContext.GetById(1, "Films");

            return Json(user);
        }


    }
}
/*            if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password))
                        {
                            // Используем фильтрацию, если переданы параметры
                            var filter = "Login = @login AND Password = @password";
                            var parameters = new Dictionary<string, object>
                    {
                        { "@login", login },
                        { "@password", password }
                    };
                            users = connectionString.ReadWithFilter<User>("Films", filter, parameters);
                        }
                        else
                        {
                            // Без фильтрации — возвращаем всех пользователей
                            users = connectionString.ReadAll<User>("Films");
                        }*/