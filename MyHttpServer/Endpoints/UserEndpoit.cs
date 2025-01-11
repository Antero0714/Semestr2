using HttpServerLibrary;
using HttpServerLibrary.Attributes;
using HttpServerLibrary.HttpResponse;
using System.Data.SqlClient;
using MyHttpServer.Models;
using MyORMLibrary;

namespace MyHttpServer.Endpoints
{

    internal class UserEndpoit : EndpointBase
    {
        [Get("users")]
        public IHttpResponseResult GetUser(string login = null, string password = null)
        {
            string connectionString = @"Data Source=localhost;Initial Catalog=user;User ID=sa;Password=P@ssw0rd;";

            using var sqlConnection = new SqlConnection(connectionString);
            var dbContext = new ORMContext<Film>(sqlConnection);

            // Укажите название таблицы
            var user = dbContext.GetById(1, "Users");

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
                            users = connectionString.ReadWithFilter<User>("Users", filter, parameters);
                        }
                        else
                        {
                            // Без фильтрации — возвращаем всех пользователей
                            users = connectionString.ReadAll<User>("Users");
                        }*/