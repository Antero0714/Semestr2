using MyORMLibrary;
using NUnit.Framework;
using TemplateEngine;
using MyORMLibrary;
namespace ORMLibrary.Tests
{
    [TestFixture]
    public class HtmlTemplateEngineTests
    {
        private HtmlTemplateEngine _templateEngine;

        [SetUp]
        public void SetUp()
        {
            _templateEngine = new HtmlTemplateEngine();
        }

        [Test]
        public void Render_ShouldReplacePlaceholderWithData()
        {
            // Arrange
            var template = "<div>{name}</div>";
            var data = "John Doe";

            // Act
            var result = _templateEngine.Render(template, data);

            // Assert
            Assert.AreEqual("<div>John Doe</div>", result);
        }

        [Test]
        public void RenderToClass_ShouldReplaceContentWithinClass()
        {
            // Arrange
            var html = "<div class=\"container\">Old Content</div>";
            var className = "container";
            var newContent = "New Content";

            // Act
            var result = _templateEngine.RenderToClass<object>(html, className, newContent);

            // Assert
            Assert.AreEqual("<div class=\"container\">New Content</div>", result);
        }

        [Test]
        public void LoadTemplate_ShouldReturnFileContent()
        {
            // Arrange
            var filePath = "testTemplate.html";
            var expectedContent = "<div>Template Content</div>";

            File.WriteAllText(filePath, expectedContent);

            // Act
            var result = _templateEngine.LoadTemplate(filePath);

            // Assert
            Assert.AreEqual(expectedContent, result);

            // Cleanup
            File.Delete(filePath);
        }
    }
}
