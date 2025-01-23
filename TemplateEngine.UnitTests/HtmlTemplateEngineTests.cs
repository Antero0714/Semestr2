
namespace TemplateEngine.UnitTests
{
    [TestFixture]
    public class HtmlTemplateEngineTests
    {
        [Test]
        public void Render_ValidTemplateAndData_ReturnHtml()
        {
            // Arrange
            IHtmlTemplateEngine engine = new HtmlTemplateEngine();
            var template = "Привет, {name}! Как дела?";
            var data = "Вася";

            // Act
            var result = engine.Render(template, data);

            // Assert
            Assert.AreEqual("Привет, Вася! Как дела?", result);
        }

     

    }
}