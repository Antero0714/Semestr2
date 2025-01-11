using HttpServerLibrary;
using HttpServerLibrary.Attributes;
using HttpServerLibrary.HttpResponse;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHttpServer.Endpoints
{
    class CommentEndpoints : EndpointBase
    {
        [Post("film/comment")]
        public IHttpResponseResult AddComment(string commentText)
        {
            try
            {
                // Получаем ID пользователя из куки
                var tokenCookie = Context.Request.Cookies.FirstOrDefault(c => c.Name == "session-token");
                if (tokenCookie == null || string.IsNullOrEmpty(tokenCookie.Value))
                {
                    return Json(new { success = false, message = "Пользователь не авторизован." });
                }

                string userId = SessionStorage.GetUserId(tokenCookie.Value);
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "Не удалось определить пользователя." });
                }

                // Подключение к БД
                using (var sqlConnection = new SqlConnection("Data Source=localhost;Initial Catalog=User;User ID=sa;Password=P@ssw0rd"))
                {
                    sqlConnection.Open();

                    // SQL-запрос для добавления комментария
                    string query = $"INSERT INTO Comments (FilmId, UserId, CommentText, CreatedAt) VALUES (2072, @UserId, @CommentText, GETDATE())";

                    using (var command = new SqlCommand(query, sqlConnection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);
                        command.Parameters.AddWithValue("@CommentText", commentText);

                    }
                }

                // Успешный ответ
                return Json(new { success = true, message = "Комментарий добавлен." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении комментария: {ex.Message}");
                return Json(new { success = false, message = "Ошибка при добавлении комментария." });
            }
        }


    }
}
