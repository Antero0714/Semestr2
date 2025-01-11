using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace TemplateEngine
{

    public class HtmlTemplateEngine : IHtmlTemplateEngine
    {
        // Заменяет {name} на значение строки
        public string Render(string template, string data)
        {
            return template.Replace("{name}", data);
        }

        public string RenderToClass<T>(string html, string className, string content)
        {
            // Ищем элемент с указанным классом
            var pattern = $"<div class=\"{className}\">(.*?)</div>";
            var replacement = $"<div class=\"{className}\">{content}</div>";

            return Regex.Replace(html, pattern, replacement, RegexOptions.Singleline);
        }

        public string LoadTemplate(string filePath)
        {
            return File.ReadAllText(filePath);
        }

        // Заменяет {propertyName} для обобщенного типа
        public string Render<T>(string template, T obj)
        {
            return Render(template, (object)obj);
        }


        // Заменяет {propertyName} на значение соответствующего свойства объекта
        public string Render(string template, object obj)
        {
            if (obj == null)
                return template;

            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Проверяем, нужно ли заменить {}
            if (template.Contains("{}"))
            {
                var genderProperty = properties.FirstOrDefault(p => p.Name.Equals("Gender", StringComparison.OrdinalIgnoreCase));
                string appeal = "Уважаемый";

                if (genderProperty != null && genderProperty.PropertyType == typeof(bool))
                {
                    var gender = (bool)genderProperty.GetValue(obj);
                    appeal = gender ? "Уважаемый" : "Уважаемая";
                }

                template = template.Replace("{}", appeal);
            }

            foreach (var property in properties)
            {
                var placeholder = $"{{{property.Name.ToLower()}}}"; // Создаем плейсхолдер {propertyName}
                var value = property.GetValue(obj)?.ToString() ?? string.Empty;
                template = template.Replace(placeholder, value);
            }

            return template;
        }


    }
}
