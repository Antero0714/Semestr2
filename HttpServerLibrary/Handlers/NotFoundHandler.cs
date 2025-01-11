namespace HttpServerLibrary.Handlers
{
    class NotFoundHandler : Handler
    {
        public override void HandleRequest(HttpRequestContext context)
        {
            // Устанавливаем статус ответа 404
            context.Response.StatusCode = 404;

            // HTML для ошибки 404
            string html = @"
                <html>
                <head>
                    <title>404 - Page not found</title>
                    <style>
                        body { font-family: Arial, sans-serif; text-align: center; padding: 50px; background-color: #f4f4f9; }
                        .error-container { max-width: 600px; margin: auto; background: #fff; padding: 20px; border-radius: 8px; box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1); }
                        a { text-decoration: none; color: #007bff; }
                        a:hover { text-decoration: underline; }
                    </style>
                </head>
                <body>
                    <div class='error-container'>
                        <h1>404 - Page not found</h1>
                        <p><a href='/main'>Return on the main page</a></p>
                    </div>
                </body>
                </html>";

            context.Response.ContentType = "text/html";
            using (var writer = new StreamWriter(context.Response.OutputStream))
            {
                writer.Write(html);
            }

            context.Response.Close();
        }
    }
}
