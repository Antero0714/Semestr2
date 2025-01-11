namespace MyHttpServer.Endpoints;
using HttpServerLibrary.HttpResponse;
using HttpServerLibrary.Attributes;
using MyHttpServer.Models;
using MyORMLibrary;
using System.Data.SqlClient;
using HttpServerLibrary;
using System.Reflection.Metadata;

internal class TemplateFilmEndpoints : EndpointBase
{
    [Get("film")]
    public IHttpResponseResult GetFilmDetails(int id)
    {
        string connectionString = @"Data Source=localhost;Initial Catalog=User;User ID=sa;Password=P@ssw0rd;";

        using var connection = new SqlConnection(connectionString);
        var ormFilm = new ORMContext<Film>(connection);
        var ormFilms = new ORMContext<TemplateFilm>(connection);

        // Получаем данные из таблиц
        var film = ormFilm.GetById(id, "Users");
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
            .Replace("{{LinkToPlayer}}", filmDetails.LinkToPlayer);

        return Html(renderedHtml);
    }

}
