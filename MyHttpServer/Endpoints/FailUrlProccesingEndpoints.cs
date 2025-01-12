using HttpServerLibrary;
using HttpServerLibrary.Attributes;
using HttpServerLibrary.HttpResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyHttpServer.Endpoints
{
    class FailUrlProccesingEndpoints : EndpointBase
    {
        [Get("*")]
        public IHttpResponseResult HandleNotFound()
        {
            // HTML-страница для 404
            string html = @"
        <html>
        <head>
            <title>404 - Страница не найдена</title>
            <style>
                body {
                    font-family: Arial, sans-serif;
                    text-align: center;
                    background-color: #f4f4f9;
                    margin: 0;
                    padding: 50px;
                }
                .error-container {
                    max-width: 600px;
                    margin: auto;
                    background: #fff;
                    padding: 20px;
                    border-radius: 8px;
                    box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
                }
                a {
                    text-decoration: none;
                    color: #007bff;
                }
                a:hover {
                    text-decoration: underline;
                }
            </style>
        </head>
        <body>
            <div class='error-container'>
                <h1>404 - Страница не найдена</h1>
                <p>К сожалению, такой страницы не существует.</p>
                <p><a href='/main'>Перейти на главную страницу</a></p>
            </div>
        </body>
        </html>";

            return Html(html);
        }


    }
}
