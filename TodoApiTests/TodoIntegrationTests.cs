using Newtonsoft.Json;
using NUnit.Framework;
using TodoApi.Models;


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

        [Test]
        public void VerifyGetTodoItem1ReturnsCorrectName()
        {
            //Arrange
            var expectedName = "Walk the dog"; //we know this is what it should be from the Controller constructor

            var url = _baseUrl + "1"; //so our URL looks like https://localhost:44350/api/Todo/1

            //Act
            var response = Utilities.SendHttpWebRequest(url, "GET"); //get the full response
            var respString = Utilities.ReadWebResponse(response); //get just the response body

            TodoItem actualTodo = JsonConvert.DeserializeObject<TodoItem>(respString); //deserialize the body string into the TodoItem object

            //Assert
            Assert.AreEqual(expectedName, actualTodo.Name, "Expected and actual names are different. Expected " + expectedName + " but was " + actualTodo.Name);
        }
    }
}
