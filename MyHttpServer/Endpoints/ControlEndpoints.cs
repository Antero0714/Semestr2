using HttpServerLibrary;
using HttpServerLibrary.Attributes;
using HttpServerLibrary.HttpResponse;
using MyHttpServer.Models;
using MyORMLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHttpServer.Endpoints
{
    internal class ControlEndpoints : EndpointBase
    {
        [Get("control")]
        public IHttpResponseResult GetControlPage()
        {
            string filePath = null;
            if(IsAuthorized(Context))
            {
                filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "public", "Templates", "Pages", "ControlPanel", "ControlPanel.html");
            }
            else
            {
                return Html("<!DOCTYPE html>\r\n<html lang=\"en\">\r\n  <head>\r\n    " +
                    "<meta charset=\"UTF-8\" />\r\n\r\n    " +
                    "<link rel=\"shortcut icon\" href=\"#\" />\r\n  </head>\r\n\r\n  <body>\r\n    " +
                    "<h1>You don't registed!</h1> <a href=\"http://localhost:8888/main\">Back to the main page.</a></body>\r\n</html>");

            }

            if (!File.Exists(filePath))
            {
                return Html("<h1>404 - File Not Found</h1>");
            }

            string content = File.ReadAllText(filePath);
            return Html(content);
        }

        [Get("films")]
        public IHttpResponseResult GetFilms()
        {
            var dbConnection = new SqlConnection("Data Source=localhost;Initial Catalog=User;User ID=sa;Password=P@ssw0rd");
            var orm = new ORMContext<Film>(dbConnection);
            var films = orm.GetByAll("Users");

            return Json(films);
        }

        [Post("films")]
        public IHttpResponseResult PostGetFilms()
        {
            var dbConnection = new SqlConnection("Data Source=localhost;Initial Catalog=User;User ID=sa;Password=P@ssw0rd");
            var orm = new ORMContext<Film>(dbConnection);
            var films = orm.GetByAll("Users");
            return Json(films);
        }




        [Post("films/create")]
        public IHttpResponseResult CreateFilm(string titleRus, string titleEng, string type, string sound, string imagePath, string link)
        {
            try
            {
                int newFilmId;

                using (var sqlConnection = new SqlConnection("Data Source=localhost;Initial Catalog=User;User ID=sa;Password=P@ssw0rd"))
                {
                    sqlConnection.Open();

                    string query = @"
                INSERT INTO Users (TitleRus, TitleEng, Type, Sound, ImagePath, Link)
                OUTPUT INSERTED.Id
                VALUES (@TitleRus, @TitleEng, @Type, @Sound, @ImagePath, @Link)";

                    using (var command = new SqlCommand(query, sqlConnection))
                    {
                        command.Parameters.AddWithValue("@TitleRus", titleRus ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TitleEng", titleEng ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Type", type ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Sound", sound ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@ImagePath", imagePath ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Link", link ?? (object)DBNull.Value);

                        newFilmId = (int)command.ExecuteScalar(); // Получаем сгенерированный Id
                    }
                }

                return Json(new { success = true, id = newFilmId });
                return Redirect(@"/control");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении фильма: {ex.Message}");
                return Json(new { success = false, message = "Не удалось добавить фильм" });
            }
        }



        [Post("films/update")]
        public IHttpResponseResult UpdateFilm(int id, string titleRus, string titleEng, string type, string sound, string imagePath, string link)
        {
            try
            {
                using (var sqlConnection = new SqlConnection("Data Source=localhost;Initial Catalog=User;User ID=sa;Password=P@ssw0rd"))
                {
                    sqlConnection.Open();

                    string query = @"
                UPDATE Users 
                SET TitleRus = @TitleRus, TitleEng = @TitleEng, Type = @Type, Sound = @Sound, ImagePath = @ImagePath, Link = @Link
                WHERE Id = @id";

                    using (var command = new SqlCommand(query, sqlConnection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@TitleRus", titleRus);
                        command.Parameters.AddWithValue("@TitleEng", titleEng);
                        command.Parameters.AddWithValue("@Type", type);
                        command.Parameters.AddWithValue("@Sound", sound);
                        command.Parameters.AddWithValue("@ImagePath", imagePath);
                        command.Parameters.AddWithValue("@Link", link);

                        command.ExecuteNonQuery();
                    }
                }

                //return Json(new { success = true });
                return Redirect(@"/control");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обновлении фильма: {ex.Message}");
                return Json(new { success = false, message = "Не удалось обновить фильм" });
            }
        }


        [Post("films/delete")]
        public IHttpResponseResult DeleteFilm(int id)
        {
            try
            {
                using (var sqlConnection = new SqlConnection("Data Source=localhost;Initial Catalog=User;User ID=sa;Password=P@ssw0rd"))
                {
                    sqlConnection.Open();

                    string query = "DELETE FROM Users WHERE Id = @id";

                    using (var command = new SqlCommand(query, sqlConnection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }

                using (var sqlConnection = new SqlConnection("Data Source=localhost;Initial Catalog=User;User ID=sa;Password=P@ssw0rd"))
                {
                    sqlConnection.Open();

                    string query = "DELETE FROM TemplateFilm WHERE Id = @id";

                    using (var command = new SqlCommand(query, sqlConnection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }

                //return Json(new { success = true });
                return Redirect(@"/control");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении фильма: {ex.Message}");
                return Json(new { success = false, message = "Не удалось удалить фильм" });
            }
        }

        [Post("films/template")]
        public IHttpResponseResult AddTemplateFilm(int id, string photo, string date, string country, string genre, string duration, string description, string linkToPlayer)
        {
            try
            {
                using (var sqlConnection = new SqlConnection("Data Source=localhost;Initial Catalog=User;User ID=sa;Password=P@ssw0rd"))
                {
                    sqlConnection.Open();

                    string query = @"
                INSERT INTO TemplateFilm (Id, Photo, Date, Country, Genre, Duration, Description, LinkToPlayer)
                VALUES (@Id, @Photo, @Date, @Country, @Genre, @Duration, @Description, @LinkToPlayer)";

                    using (var command = new SqlCommand(query, sqlConnection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        command.Parameters.AddWithValue("@Photo", photo ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Date", date ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Country", country ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Genre", genre ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Duration", duration ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Description", description ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@LinkToPlayer", linkToPlayer ?? (object)DBNull.Value);

                        command.ExecuteNonQuery();
                    }
                }

                //return Json(new { success = true });
                return Redirect(@"/control");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении информации о фильме: {ex.Message}");
                return Json(new { success = false, message = "Не удалось сохранить информацию о фильме" });
            }
        }


        private bool IsAuthorized(HttpRequestContext context)
        {
            if (context.Request.Cookies.Any(c => c.Name == "session-token"))
            {
                var cookie = context.Request.Cookies["session-token"];
                Console.WriteLine($"Cookie Value: {cookie.Value}");

                // Проверяем токен через SessionStorage
                bool isValid = SessionStorage.ValidateToken(cookie.Value);
                Console.WriteLine($"Is Token Valid: {isValid}");
                return isValid;
            }

            Console.WriteLine("No session-token cookie found.");
            return false;
        }



    }
}
