using HttpServerLibrary;
using HttpServerLibrary.Attributes;
using HttpServerLibrary.Configurations;
using HttpServerLibrary.HttpResponse;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHttpServer.Endpoints
{
    internal class RateEndpoints : EndpointBase
    {
        [Post("film/rate")]
        public IHttpResponseResult RateFilm(int filmId, int ratingValue)
        {
            // Проверяем, что значение рейтинга находится в допустимых пределах
            if (ratingValue < 1 || ratingValue > 5)
            {
                return Json(new { success = false, message = "Недопустимое значение рейтинга." });
            }

            // Получаем токен из куки
            var token = Context.Request.Cookies["session-token"]?.Value;

            if (string.IsNullOrEmpty(token))
            {
                return Json(new { success = false, message = "Необходимо авторизоваться для оценки фильма." });
            }

            // Получаем ID пользователя по токену
            var userId = SessionStorage.GetUserId(token);

            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "Сессия недействительна или истекла." });
            }

            try
            {
                string connectionString = AppConfig.Instance.ConnectionString;

                using (var sqlConnection = new SqlConnection(AppConfig.Instance.ConnectionString))
                {
                    sqlConnection.Open();

                    // Проверяем, есть ли уже оценка от пользователя для этого фильма
                    string checkQuery = @"
                SELECT COUNT(*) 
                FROM Ratings 
                WHERE FilmId = @FilmId AND UserId = @UserId";

                    using (var checkCommand = new SqlCommand(checkQuery, sqlConnection))
                    {
                        checkCommand.Parameters.AddWithValue("@FilmId", filmId);
                        checkCommand.Parameters.AddWithValue("@UserId", int.Parse(userId));
                        int existingRatings = (int)checkCommand.ExecuteScalar();

                        if (existingRatings > 0)
                        {
                            // Обновляем существующий рейтинг
                            string updateQuery = @"
                        UPDATE Ratings
                        SET RatingValue = @RatingValue
                        WHERE FilmId = @FilmId AND UserId = @UserId";

                            using (var updateCommand = new SqlCommand(updateQuery, sqlConnection))
                            {
                                updateCommand.Parameters.AddWithValue("@RatingValue", ratingValue);
                                updateCommand.Parameters.AddWithValue("@FilmId", filmId);
                                updateCommand.Parameters.AddWithValue("@UserId", int.Parse(userId));
                                updateCommand.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            // Вставляем новый рейтинг
                            string insertQuery = @"
                        INSERT INTO Ratings (FilmId, UserId, RatingValue)
                        VALUES (@FilmId, @UserId, @RatingValue)";

                            using (var insertCommand = new SqlCommand(insertQuery, sqlConnection))
                            {
                                insertCommand.Parameters.AddWithValue("@FilmId", filmId);
                                insertCommand.Parameters.AddWithValue("@UserId", int.Parse(userId));
                                insertCommand.Parameters.AddWithValue("@RatingValue", ratingValue);
                                insertCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }

                // Возвращаем успешный результат
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении рейтинга: {ex.Message}");
                return Json(new { success = false });
            }
        }


    }
}
