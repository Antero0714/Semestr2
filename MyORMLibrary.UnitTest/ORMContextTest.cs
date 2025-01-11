//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.Data.SqlClient;
//using NUnit.Framework;
//using MyORMLibrary.UnitTest.Models;
//using Moq;
//using MyORMLibrary;
//using MyORMLibrary.UnitTest;

//namespace MyORMLibrary.UnitTest
//{
//    [TestClass]
//    public class ORMContextTest
//    {
//        //Arrange
//        //Act
//        //Accert

//        public readonly ORMContext<UserInfo> _dbContext;
//        private Mock<SqlConnection> _sqlCommand;
//        private Mock<SqlDataReader> _sqlDataReader;

//        public ORMContextTest()
//        {
//            _dbContext = new ORMContext<UserInfo>("ConnectionString");
//        }

//        [SetUp]
//        public void Setup()
//        {
//            //var fakeSqlConnection = new SqlConnection("ConnectionString");
//            var _sqlConnection = new Mock<SqlConnection>();
//            _sqlCommand = new Mock<SqlCommand>();
//            _sqlDataReader = new Mock<SqlDataReader>();

//            _sqlCommand.Setup(x => x.Connection).Returns(SqlConnection.Object);

//            _dbContext = new ORMContext<UserInfo>("ConnectionString");
//            _dbContext.SqlConnection = sqlConnection.Object;
//        }


//        [Test]
//        public void GetById_When_()
//        {
//            //Arrange
//            var userInfo = new UserInfo()
//            {
//                Id = 1,
//                Age = 28,
//                Email = "name@mail.ru",
//                Name = "Иванов Иван Иванович",
//                Gender = true
//            };



//            //Act
//            var result = _dbContext.GetById(1);

//            //Assert
//            NUnit.Framework.Assert.Pass();
//        }





//        public void Test_CreateUser()
//        {
//            var orm = new ORMContext("YourConnectionString");
//            var user = new User { Login = "Test", Password = "1234" };

//            var result = orm.Create(user, "Users");

//            Assert.IsNotNull(result);
//            Assert.AreEqual("Test", result.Login);
//        }

//    }
//}
