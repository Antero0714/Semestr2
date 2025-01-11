using System.Net;
using HttpServerLibrary.Handlers;

namespace HttpServerLibrary;

public sealed class HttpServer
{
    private readonly HttpListener _listener;
    private readonly string _staticDirectoryPath;

    private readonly Handler _staticFilesHandler;
    private readonly Handler _endpointsHandler;

    public HttpServer(string[] prefixes, string staticDirectoryPath)
    {
        _listener = new HttpListener();
        _staticDirectoryPath = staticDirectoryPath;

        foreach (string prefix in prefixes)
        {
            Console.WriteLine($"Добавлен префикс: {prefix}");
            _listener.Prefixes.Add(prefix);
        }

        // Инициализация обработчиков
        _staticFilesHandler = new StaticFilesHandler(_staticDirectoryPath);
        _endpointsHandler = new EndpointsHandler();
        var notFoundHandler = new NotFoundHandler();

        // Устанавливаем цепочку ответственности
        _staticFilesHandler.Successor = _endpointsHandler;
        _endpointsHandler.Successor = notFoundHandler;
    }

    /// <summary>
    /// Запуск сервера
    /// </summary>
    public async Task StartAsync()
    {
        _listener.Start();
        Console.WriteLine("Сервер запущен и ожидает запросов...");

        while (_listener.IsListening)
        {
            try
            {
                var context = await _listener.GetContextAsync();
                var requestContext = new HttpRequestContext(context.Request, context.Response);

                // Обработка запроса
                await ProcessRequest(requestContext);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обработке запроса: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Обработка запроса через цепочку обработчиков
    /// </summary>
    private async Task ProcessRequest(HttpRequestContext context)
    {
        _staticFilesHandler.HandleRequest(context);
    }

    /// <summary>
    /// Остановка сервера
    /// </summary>
    public void Stop()
    {
        _listener.Stop();
        Console.WriteLine("Сервер остановлен.");
    }
}

/// <summary>
/// Контекст для передачи данных между обработчиками
/// </summary>

