namespace TemplateEngine
{
    public interface IHtmlTemplateEngine
    {
        // string string
        // string object
        //string T
        string Render(string template, string data);

        string Render(string template, object obj);


        string Render<T>(string template, T obj);
    }
}
