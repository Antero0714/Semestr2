using HttpServerLibrary.Configurations;
using MyORMLibrary;
using MyHttpServer.Models;
using System.Data.SqlClient;
using TemplateEngine;

namespace HttpServerLibrary
{
    internal class Program
    {

        static async Task Main(string[] args)
        {
            Console.WriteLine("Сервер запускается...");
            await StartServerAsync();
        }

        #region
        /*static async Task Main(string[] args)
        {
            var engine = new HtmlTemplateEngine();

            // Загружаем HTML-шаблон
            string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "public", "looktoon", "index.html");
            string template = File.ReadAllText(templatePath);

            // Получаем данные из базы через ORMContext
            var cartoons = GetCartoons();

            // Генерируем HTML-код для мультфильмов
            var cartoonsHtml = "";
            foreach (var cartoon in cartoons)
            {
                cartoonsHtml += $@"
        <div class='indicator'>
            <div class='live_contain'>
                <div class='live_premiere2'>Премьера</div>
                <a href='{cartoon.Link}'>
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

            // Вставляем сгенерированный HTML в элемент с классом cartoons-container
            var finalHtml = engine.RenderToClass<Film>(template, "cartoons-container", cartoonsHtml);

            // Сохраняем результат обратно в index.html
            string outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "public", "looktoon", "index.html");
            File.WriteAllText(outputPath, finalHtml);

            Console.WriteLine("HTML успешно сгенерирован!");

            // Логика сервера (если требуется) остаётся без изменений
            await StartServerAsync();
        }*/
        #endregion

        static async Task StartServerAsync()
        {
            // Загружаем конфигурацию через Singleton
            var config = AppConfig.Instance;

            // Проверяем наличие статического каталога
            if (!Directory.Exists(config.StaticDirectoryPath))
            {
                Console.WriteLine($"Статическая директория \"{config.StaticDirectoryPath}\" не найдена.");
                Directory.CreateDirectory(config.StaticDirectoryPath);
                Console.WriteLine($"Создана новая директория: \"{config.StaticDirectoryPath}\"");
            }

            // Запуск сервера
            try
            {
                var server = new HttpServer(new[] { $"http://{config.Domain}:{config.Port}/" }, config.StaticDirectoryPath);

                Console.WriteLine($"Сервер запущен на http://{config.Domain}:{config.Port}/");
                await server.StartAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при запуске сервера: {ex.Message}");
            }
        }




        /*public void GenerateHtml()
        {
            var engine = new HtmlTemplateEngine();

            // Загружаем шаблон
            var template = engine.LoadTemplate("C:\\Users\\andre\\Desktop\\Semestrovka\\MyHttpServer\\public\\looktoon\\index.html");

            // Пример данных
            var cartoons = new List<Users>
    {
        new Users
        {
            Link = "adultswim/chicken_for_linda.html",
            ImagePath = "looktoon/imgscreen/ChickenforLinda.jpg",
            TitleRus = "Цыплёнок для Линды!",
            TitleEng = "Chicken for Linda!",
            Type = "Полнометражный фильм",
            Sound = "Субтитры"
        },
        new Users
        {
            Link = "cartoons/tom_and_jerry.html",
            ImagePath = "looktoon/imgscreen/TomAndJerry.jpg",
            TitleRus = "Том и Джерри",
            TitleEng = "Tom and Jerry",
            Type = "Сериал",
            Sound = "Озвучка"
        }
    };

            // Генерируем HTML
            var html = engine.RenderList(template, cartoons);

            // Сохраняем результат в файл
            File.WriteAllText("output.html", html);
        }*/
    }
}
