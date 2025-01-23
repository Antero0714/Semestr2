using HttpServerLibrary;
using HttpServerLibrary.Attributes;
using HttpServerLibrary.Configurations;
using HttpServerLibrary.HttpResponse;
using System.Data.SqlClient;
using System.Net;

namespace MyHttpServer.Endpoints
{
    class RegisterEndpoint : EndpointBase
    {

        [Get("ENTRY")]
        public IHttpResponseResult GetEntryPage()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "public", "Templates", "Pages", "Auth", "signin.html");

            if (!File.Exists(filePath))
            {
                return Html("<h1>404 - File Not Found</h1>");
            }

            string content = File.ReadAllText(filePath);
            return Html(content);
        }

        [Get("register")]
        public IHttpResponseResult GetRegisterPage()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "public", "Templates", "Pages", "Auth", "signup.html");

            if (!File.Exists(filePath))
            {
                return Html("<h1>404 - File Not Found</h1>");
            }

            string content = File.ReadAllText(filePath);
            return Html(content);
        }

        [Post("exit")]
        public IHttpResponseResult ExitFromAccount()
        {
            DeleteCookieIfAvtoriz(context:Context);
            return Redirect(@"/main");
        }

        [Post("main")]
        public IHttpResponseResult Login(string username, string email, string password)
        {

            DeleteCookieIfAvtoriz(Context);
/*            // Проверяем входные данные
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
            {
                var loginPage = File.ReadAllText(@"public\looktoon\");
                return Html(loginPage);
            }*/

            username = username.Trim();
            password = password.Trim();
            email = email.Trim();


            using var sqlConnection = new SqlConnection(AppConfig.GetInstance().ConnectionStrings["DefaultConnection"]);

            sqlConnection.Open();

            string query = "SELECT Name FROM TUserDash WHERE Name = @Name AND Password = @Password AND Email = @Email";
            
            using (var command = new SqlCommand(query, sqlConnection))
            {
                command.Parameters.AddWithValue("@Name", username);
                command.Parameters.AddWithValue("@Password", password);
                command.Parameters.AddWithValue("@Email", email);
                
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Получаем имя пользователя
                        string name = reader["Name"].ToString();
                        string token = Guid.NewGuid().ToString(); // Генерируем токен

                        // Сохраняем токен и имя пользователя в памяти
                        SessionStorage.SaveSession(token, name);

                        // Устанавливаем куку с токеном
                        var cookie = new Cookie("session-token", token)
                        {
                            HttpOnly = true,
                            Secure = false,
                            Path = "/" // Доступно на всём сайте
                        };
                        Context.Response.Cookies.Add(cookie);

                        /*         // Загружаем HTML-шаблон и заменяем плейсхолдер
                                 var dashboardPage = File.ReadAllText(@"Templates\Pages\Dashboard\index.html");
                                 dashboardPage = dashboardPage.Replace("{{UserName}}", name);*/

                        Console.WriteLine("ВСЁ ОК С КУКИ");  

                        return Redirect(@"/main");

                    }
                }
            }
            Console.WriteLine("не прошли данные куки");
            return Redirect(@"/ENTRY");
        }


        [Post("register")]
        public IHttpResponseResult Register(string username, string password, string email)
        {
            DeleteCookieIfAvtoriz(Context);
            // Проверяем входные данные
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
            {
                var loginPage = File.ReadAllText(@"public\looktoon\index.html");
                return Html(loginPage);
            }

            username = username.Trim();
            password = password.Trim();
            email = email.Trim();

            string connectionString = @"Data Source=localhost;Initial Catalog=User;User ID=sa;Password=P@ssw0rd";

            using var sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            // Проверяем, существует ли пользователь с таким именем
            string checkUsernameQuery = "SELECT COUNT(*) FROM TUserDash WHERE Name = @Name";
            using (var command = new SqlCommand(checkUsernameQuery, sqlConnection))
            {
                command.Parameters.AddWithValue("@Name", username);
                int userCount = (int)command.ExecuteScalar();
                if (userCount > 0)
                {
                    Console.WriteLine("УЖЕ ЕСТЬ ТАКОЕ ИМЯ");
                    return Redirect(@"/register");
                }
            }

            // Проверяем, существует ли email
            string checkEmailQuery = "SELECT COUNT(*) FROM TUserDash WHERE Email = @Email";
            using (var command = new SqlCommand(checkEmailQuery, sqlConnection))
            {
                command.Parameters.AddWithValue("@Email", email);
                int emailCount = (int)command.ExecuteScalar();
                if (emailCount > 0)
                {
                    Console.WriteLine("УЖЕ ЕСТЬ ТАКОЙ ЕМАИЛ");
                    return Redirect(@"/register");
                }
            }

            // Если все проверки прошли, добавляем нового пользователя
            string insertUserQuery = "INSERT INTO TUserDash (Name, Password, Email) VALUES (@Name, @Password, @Email)";
            using (var insertCommand = new SqlCommand(insertUserQuery, sqlConnection))
            {
                insertCommand.Parameters.AddWithValue("@Name", username);
                insertCommand.Parameters.AddWithValue("@Password", password);
                insertCommand.Parameters.AddWithValue("@Email", email);
                insertCommand.ExecuteNonQuery();
            }

            // После успешной регистрации перенаправляем на страницу логина
            Console.WriteLine("НАПРАВЛЯЕМ ДАЛЬШЕ, ТЫ ЗАРЕГЕСТРИРОВАЛСЯ");

            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "public", "looktoon", "index.html");
            string content = File.ReadAllText(filePath);
            content = content.Replace("{{EntryOrName}}", username);

        

            string query = "SELECT Name FROM TUserDash WHERE Name = @Name AND Password = @Password AND Email = @Email";

            using (var command = new SqlCommand(query, sqlConnection))
            {
                command.Parameters.AddWithValue("@Name", username);
                command.Parameters.AddWithValue("@Password", password);
                command.Parameters.AddWithValue("@Email", email);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Получаем имя пользователя
                        string name = reader["Name"].ToString();
                        string token = Guid.NewGuid().ToString(); // Генерируем токен

                        // Сохраняем токен и имя пользователя в памяти
                        SessionStorage.SaveSession(token, name);

                        // Устанавливаем куку с токеном
                        var cookie = new Cookie("session-token", token)
                        {
                            HttpOnly = true,
                            Secure = false,
                            Path = "/" // Доступно на всём сайте
                        };
                        Context.Response.Cookies.Add(cookie);

                        /*         // Загружаем HTML-шаблон и заменяем плейсхолдер
                                 var dashboardPage = File.ReadAllText(@"Templates\Pages\Dashboard\index.html");
                                 dashboardPage = dashboardPage.Replace("{{UserName}}", name);*/

                        Console.WriteLine("ВСЁ ОК С КУКИ");

                        return Redirect(@"/main");

                    }
                }
            }
            return Redirect(@"/main");
        }

        private bool DeleteCookieIfAvtoriz(HttpRequestContext context)
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

                    // Удаляем куки, если пользователь зарегистрирован
                    SessionStorage.DeleteSession(cookie.Value);
                    Console.WriteLine("session-token cookie deleted.");

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
    }
}
