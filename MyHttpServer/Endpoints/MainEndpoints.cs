using HttpServerLibrary;
using HttpServerLibrary.Attributes;
using HttpServerLibrary.Configurations;
using HttpServerLibrary.HttpResponse;
using Microsoft.AspNetCore.Http;
using MyHttpServer.Models;
using MyORMLibrary;
using System.Data.SqlClient;
using TemplateEngine;

namespace MyHttpServer.Endpoints
{
    internal class MainEndpoints : EndpointBase
    {

        /*[Post("main")]
        public async Task<IHttpResponseResult> Login(string uname, string email, string psw)
        {
            try
            {
                using (var dbConnection = new SqlConnection(@"Data Source=localhost;Initial Catalog=user;User ID=sa;Password=P@ssw0rd;"))
                {
                    var ormContext = new ORMContext<UserReg>(dbConnection);
                    var users = ormContext.GetByAll("usersAvtoriz");

                    var user = users.FirstOrDefault(u => u.Name == uname && u.Email == email && u.Password == psw);
                    if (user != null)
                    {
                        return Json(new { success = true });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Неверные учетные данные." });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Ошибка сервера: " + ex.Message });
            }
        }*/
        private bool IsAuthorized(HttpRequestContext context)
        {
            string userName = null;

            if (context.Request.Cookies.Any(c => c.Name == "session-token"))
            {
                var cookie = context.Request.Cookies["session-token"];
                Console.WriteLine($"Cookie Value: {cookie.Value}");

                // Проверяем токен через SessionStorage
                if (SessionStorage.ValidateToken(cookie.Value))
                {
                    userName = SessionStorage.GetUserNameByToken(cookie.Value); // Получаем имя пользователя по токену
                    Console.WriteLine($"User Name: {userName}");
                    return true;
                }

                Console.WriteLine("Invalid session-token.");
            }
            else
            {
                Console.WriteLine("No session-token cookie found.");
            }

            return false;
        }

        [Get("main")]
        public IHttpResponseResult MainPage()
        {
            // Загружаем HTML-шаблон
            string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "public", "looktoon", "index.html");
            string template = File.ReadAllText(templatePath);

            // Получаем токен из куки
            var tokenCookie = Context.Request.Cookies.FirstOrDefault(c => c.Name == "session-token");
            string username = "Вход"; // Значение по умолчанию

            if (tokenCookie != null)
            {
                // Проверяем токен и извлекаем имя пользователя
                var token = tokenCookie.Value;
                username = SessionStorage.GetUserNameByToken(token) ?? "Вход";
            }
            // Заменяем {{EntryOrName}} на имя пользователя
            template = template.Replace("{{EntryOrName}}", username);

            if (IsAuthorized(Context))
            {
                template = template.Replace("{{CheckControl}}", "<button class=\"open-modal-btn\" onclick=\"window.location.href='/control';\">\r\n                Панель управления\r\n            </button>");
            }
            else
            {
                template = template.Replace("{{CheckControl}}", "");
            }

            // Получаем мультфильмы
            var cartoons = GetCartoons();

            // Генерируем HTML для мультфильмов
            var cartoonsHtml = "";
            foreach (var cartoon in cartoons)
            {
                cartoonsHtml += $@"
    <div class='indicator'>
        <div class='live_contain'>
            <div class='live_premiere2'>{cartoon.Year}</div>
            <a href='/film?id={cartoon.Id}'>
                <span class='live_image'>
                    <img src='{cartoon.ImagePath}' alt='{cartoon.TitleRus}'>
                </span>
                <span class='live_namerus'>{cartoon.TitleRus}</span>
                <br>
                <span class='live_nameeng'>{cartoon.TitleEng}</span>
                <br>
                <span class='live_series'>{cartoon.Type}</span>
                <div class='live_sound2'>{cartoon.Sound}</div>
            </a>
        </div>
    </div>
    <p></p>
    <p></p>";
            }

            // Получаем категории мультфильмов
            var categories = GetAnimationCategories();

            // Генерируем HTML для категорий
            var categoriesHtml = "";
            foreach (var category in categories)
            {
                categoriesHtml += $@"
    <li>
        <a href='{category.Link}' title='{category.Description}'>
            {category.Name}
        </a>
        <div class='{category.Label}'></div>
    </li>
    <br>";
            }

            // Вставляем мультфильмы и категории в шаблон
            var engine = new HtmlTemplateEngine();
            template = engine.RenderToClass<Film>(template, "cartoons-container", cartoonsHtml);
            var finalHtml = engine.RenderToClass<Category>(template, "menubox", categoriesHtml);

            return Html(finalHtml);
        }

        private List<Category> GetAnimationCategories()
        {
            using (var sqlConnection = new SqlConnection("Data Source=localhost;Initial Catalog=User;User ID=sa;Password=P@ssw0rd"))
            {
                sqlConnection.Open();

                string query = "SELECT Name, Description, Link, Label FROM AnimationCategories";
                using (var command = new SqlCommand(query, sqlConnection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        var categories = new List<Category>();
                        while (reader.Read())
                        {
                            categories.Add(new Category
                            {
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"].ToString(),
                                Link = reader["Link"].ToString(),
                                Label = reader["Label"].ToString()
                            });
                        }
                        return categories;
                    }
                }
            }
        }

        static public List<Film> GetCartoons()
        {
            using (var connection = new SqlConnection(@"Data Source=localhost;Initial Catalog=user;User ID=sa;Password=P@ssw0rd;"))
            {
                var ormContext = new ORMContext<Film>(connection);
                return ormContext.GetByAll("Users");
            }
        }



        [Get("mult")]
        public IHttpResponseResult GetMultPage()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "public", "looktoonKungFu", "index.html");

            if (!File.Exists(filePath))
            {
                return Html("<h1>404 - File Not Found</h1>");
            }

            string content = File.ReadAllText(filePath);
            return Html(content);
        }
    }
}