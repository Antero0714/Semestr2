namespace MyHttpServer.Endpoints;
using HttpServerLibrary.HttpResponse;
using HttpServerLibrary.Attributes;
using MyHttpServer.Models;
using MyORMLibrary;
using System.Data.SqlClient;
using HttpServerLibrary;
using System.Reflection.Metadata;
using HttpServerLibrary.Configurations;

internal class TemplateFilmEndpoints : EndpointBase
{
    [Get("film")]
    public IHttpResponseResult GetFilmDetails(int id)
    {
        string connectionString = AppConfig.GetInstance().ConnectionStrings["DefaultConnection"];
        using var connection = new SqlConnection(connectionString);
        var ormFilm = new ORMContext<Film>(connection);
        var ormFilms = new ORMContext<TemplateFilm>(connection);

        // Получаем данные из таблиц
        var film = ormFilm.GetById(id, "Films");
        var filmDetails = ormFilms.GetById(id, "TemplateFilm");

        if (film == null || filmDetails == null)
        {
            return Html("<h1>Film doesn't exist</h1>");
        }
        // C: \Users\andre\Desktop\Semestrovka\MyHttpServer\public\looktoonKungFu\TemplateHtml.html
        // Загружаем HTML-шаблон
        string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "public", "looktoonKungFu", "TemplateHtml.html");
        string template = File.ReadAllText(templatePath);

        // Подставляем значения                     
        string renderedHtml = template
            .Replace("{{TitleRus}}", film.TitleRus)
            .Replace("{{TitleEng}}", film.TitleEng)
            .Replace("{{Type}}", film.Type)
            .Replace("{{Photo}}", filmDetails.Photo)
            .Replace("{{Date}}", filmDetails.Date)
            .Replace("{{Country}}", filmDetails.Country)
            .Replace("{{Genre}}", filmDetails.Genre)
            .Replace("{{Duration}}", filmDetails.Duration)
            .Replace("{{Description}}", filmDetails.Description)
            .Replace("{{LinkToPlayer}}", filmDetails.LinkToPlayer)
            ;

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
        renderedHtml = renderedHtml.Replace("{{EntryOrName}}", username);
        if (IsAuthorized(Context))
        {
            renderedHtml = renderedHtml.Replace("{{CheckControl}}", "<button class=\"open-modal-btn\" onclick=\"window.location.href='/control';\">\r\n                Панель управления\r\n            </button>");
        }
        else
        {
            renderedHtml = renderedHtml.Replace("{{CheckControl}}", "");

        }


        return Html(renderedHtml);
    }

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
}
