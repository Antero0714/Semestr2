using TemplateEngine.UnitTests.Models;

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

        [Test]
        public void Render_ValidObject_ReturnHtml()
        {
            // Arrange
            IHtmlTemplateEngine engine = new HtmlTemplateEngine();
            var student = new Student { Id = 1, Name = "Вася" };
            var template = "Ура вы поступили, {name}! Ваш номер студенческого билета: {id}";


            // Act
            var result = engine.Render(template, student);

            // Assert
            Assert.AreEqual("Ура вы поступили, Вася! Ваш номер студенческого билета: 1", result);
        }

        [Test]
        public void Render_ValidAppealM_ReturnHtml()
        {
            // Arrange
            IHtmlTemplateEngine engine = new HtmlTemplateEngine();
            var student = new Student { Id = 1, Name = "Вася", Gender = true };
            var template = "{}, {name}! Ваш номер студенческого билета: {id}";


            // Act
            var result = engine.Render(template, student);

            // Assert
            Assert.AreEqual("Уважаемый, Вася! Ваш номер студенческого билета: 1", result);
        }

        [Test]
        public void Render_ValidAppealF_ReturnHtml()
        {
            // Arrange
            IHtmlTemplateEngine engine = new HtmlTemplateEngine();
            var student = new Student { Id = 1, Name = "Александра", Gender = false };
            var template = "{}, {name}! Ваш номер студенческого билета: {id}";


            // Act
            var result = engine.Render(template, student);

            // Assert
            Assert.AreEqual("Уважаемая, Александра! Ваш номер студенческого билета: 1", result);
        }
    }
}