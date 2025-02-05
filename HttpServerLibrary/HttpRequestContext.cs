﻿using System.Net;

namespace HttpServerLibrary
{
    public sealed class HttpRequestContext
    {
        public HttpListenerRequest Request { get; }

        public HttpListenerResponse Response { get; }

        public HttpRequestContext(HttpListenerRequest request, HttpListenerResponse response)
        {
            Request = request;
            Response = response;
        }
    }
}
