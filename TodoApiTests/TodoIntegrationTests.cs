using NUnit.Framework;

namespace TodoApiTests
{
    [TestFixture]
    public class TodoIntegrationTests
    {
        private static string _baseUrl;

        [OneTimeSetUp]
        public void TestClassInitialize()
        {
            //make sure this has the correct port!
            _baseUrl = "https://localhost:44350/api/Todo/";
        }

        [Test]
        public void VerifyGetReturns200Ok()
        {
            //Arrange
            //nothing to arrange

            //Act
            var response = Utilities.SendHttpWebRequest(_baseUrl, "GET");

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode, "Get method did not return a success status code; it returned " + response.StatusCode);
        }
    }
}
