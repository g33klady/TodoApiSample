using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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

        [Test]
        public void VerifyGetTodoItemsReturns3Items() //the results of this test will change depending on execution order. THIS IS A BAD TEST!
        {
            //Arrange
            int expectedCount = 3;

            //Act
            var response = Utilities.SendHttpWebRequest(_baseUrl, "GET");
            var respString = Utilities.ReadWebResponse(response);

            List<TodoItem> todoList = JsonConvert.DeserializeObject<List<TodoItem>>(respString); //we're doing a get of all items, so we'll have a List of TodoItem objects to deal with

            //Assert
            Assert.IsTrue(todoList.Count == expectedCount, "Actual count was not " + expectedCount + " it was " + todoList.Count);
        }

        [Test]
        public void VerifyPostingTodoItemPostsTheItem()
        {
            //Arrange
            TodoItem expItem = new TodoItem
            {
                Name = "mow the lawn",
                DateDue = new DateTime(2019, 12, 31),
                IsComplete = false
            };
            var reqBody = JsonConvert.SerializeObject(expItem); //we need to turn this object into a JSON string

            //Act
            //first we need to do the POST action, and get the new object's ID
            var postResponse = Utilities.SendHttpWebRequest(_baseUrl, "POST", reqBody);
            var postRespString = Utilities.ReadWebResponse(postResponse);
            TodoItem postRespItem = JsonConvert.DeserializeObject<TodoItem>(postRespString);
            var newItemId = postRespItem.Id; //we need the ID to do the GET request of this item

            //now we need to do the GET action, using the new object's ID
            var getResponse = Utilities.SendHttpWebRequest(_baseUrl + newItemId, "GET");
            var getRespString = Utilities.ReadWebResponse(getResponse);
            TodoItem getRespItem = JsonConvert.DeserializeObject<TodoItem>(getRespString);

            //Assert
            Assert.AreEqual(expItem.Name, getRespItem.Name, "Item Names are not the same, expected " + expItem.Name + " but got " + getRespItem.Name);
            Assert.AreEqual(expItem.DateDue, getRespItem.DateDue, "Item DateDue are not the same, expected " + expItem.DateDue + " but got " + getRespItem.DateDue);
            Assert.AreEqual(expItem.IsComplete, getRespItem.IsComplete, "Item IsComplete are not the same, expected " + expItem.IsComplete + " but got " + getRespItem.IsComplete);
        }
    }
}
