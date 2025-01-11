using Moq;
using MyORMLibrary;
using NUnit.Framework;
using System.Data;

namespace ORMLibrary.Tests
{
    [TestFixture]
    public class ORMContextTests
    {
        private Mock<IDbConnection> _dbConnection;
        private Mock<IDbCommand> _dbCommand;
        private Mock<IDataReader> _dbDataReader;
        private Mock<IDataParameterCollection> _dataParameterCollection;
        private ORMContext<TUserDash> _dbContext;

        [SetUp]
        public void SetUp()
        {
            _dbConnection = new Mock<IDbConnection>();
            _dbCommand = new Mock<IDbCommand>();
            _dbDataReader = new Mock<IDataReader>();
            _dataParameterCollection = new Mock<IDataParameterCollection>();

            _dbContext = new ORMContext<TUserDash>(_dbConnection.Object);
        }

        [Test]
        public void GetById_ShouldReturnEntity_WhenIdExists()
        {
            // Arrange
            var userId = 1;
            var user = new TUserDash
            {
                Id = userId,
                Name = "Test User",
                Password = "hashed_password",
                Email = "testuser@example.com"
            };

            // Настраиваем Mock<IDataReader>
            _dbDataReader.SetupSequence(r => r.Read()).Returns(true).Returns(false);
            _dbDataReader.Setup(r => r["Id"]).Returns(user.Id);
            _dbDataReader.Setup(r => r["Name"]).Returns(user.Name);
            _dbDataReader.Setup(r => r["Password"]).Returns(user.Password);
            _dbDataReader.Setup(r => r["Email"]).Returns(user.Email);

            // Настраиваем Mock<IDbCommand>
            var parameterMock = new Mock<IDbDataParameter>();
            _dbCommand.Setup(c => c.CreateParameter()).Returns(parameterMock.Object); // Настройка CreateParameter
            _dbCommand.Setup(c => c.Parameters).Returns(_dataParameterCollection.Object);
            _dbCommand.Setup(c => c.ExecuteReader()).Returns(_dbDataReader.Object);

            _dbConnection.Setup(c => c.CreateCommand()).Returns(_dbCommand.Object);

            // Act
            var result = _dbContext.GetById(userId, "TUserDash");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(user.Id, result.Id);
            Assert.AreEqual(user.Name, result.Name);
            Assert.AreEqual(user.Password, result.Password);
            Assert.AreEqual(user.Email, result.Email);
        }

        [Test]
        public void Create_ShouldExecuteInsertQuery()
        {
            // Arrange
            var user = new TUserDash
            {
                Name = "New User",
                Password = "new_password",
                Email = "newuser@example.com"
            };

            // Настройка мока параметра
            var parameterMock = new Mock<IDbDataParameter>();

            _dbCommand.Setup(c => c.Parameters).Returns(_dataParameterCollection.Object);
            _dbCommand.Setup(c => c.CreateParameter()).Returns(parameterMock.Object); // Возвращаем мок параметра
            _dbCommand.Setup(c => c.ExecuteNonQuery()).Verifiable();

            _dbConnection.Setup(c => c.CreateCommand()).Returns(_dbCommand.Object);

            // Act
            _dbContext.Create(user, "TUserDash");

            // Assert
            _dbCommand.Verify(c => c.ExecuteNonQuery(), Times.Once);
        }


        // Модель данных для тестов
        public class TUserDash
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
        }
    }
}