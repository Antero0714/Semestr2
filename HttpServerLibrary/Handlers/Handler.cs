namespace HttpServerLibrary.Handlers
{
    abstract class Handler
    {
        public Handler Successor { get; set; }

        public abstract void HandleRequest(HttpRequestContext context);
    }

}
