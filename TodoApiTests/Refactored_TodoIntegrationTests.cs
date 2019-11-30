using NUnit.Framework;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using TodoApi.Models;


namespace TodoApiTests
{
    [TestFixture]
    public class Refactored_TodoIntegrationTests
    {
        private static string _baseUrl;
        private static RestClient _client;

        [OneTimeSetUp]
        public void TestClassInitialize()
        {
            //make sure this has the correct port!
            _baseUrl = "https://localhost:44350/api/Todo/";
            _client = new RestClient(_baseUrl);
        }

        [Test]
        public void VerifyGetReturns200Ok()
        {
            //Arrange
            var request = Helpers.GetAllTodoItemsRequest();

            //Act
            IRestResponse response = _client.Execute(request);

            //Assert
            Assert.IsTrue(response.IsSuccessful, "Get method did not return a success status code; it returned " + response.StatusCode);
        }

        [Test]
        public void VerifyGetTodoItem1ReturnsCorrectName()
        {
            //Arrange
            var expectedName = "Walk the dog"; //we know this is what it should be from the Controller constructor
            var request = Helpers.GetSingleTodoItemRequest(1);

            //Act
            IRestResponse<TodoItem> actualTodo = _client.Execute<TodoItem>(request);

            //Assert
            Assert.AreEqual(expectedName, actualTodo.Data.Name, "Expected and actual names are different. Expected " + expectedName + " but was " + actualTodo.Data.Name);
        }

        [Test]
        public void VerifyPostingTodoItemPostsTheItem()
        {
            //Arrange
            var expItem = Helpers.GetTestTodoItem();
            var postRequest = Helpers.PostTodoItemRequest(expItem);

            //Act
            //first we need to do the POST action, and get the new object's ID
            IRestResponse<TodoItem> postTodo = _client.Execute<TodoItem>(postRequest);
            var newItemId = postTodo.Data.Id; //we need the ID to do the GET request of this item

            //now we need to do the GET action, using the new object's ID
            var getRequest = Helpers.GetSingleTodoItemRequest(newItemId);
            IRestResponse<TodoItem> getTodo = _client.Execute<TodoItem>(getRequest);

            //Assert
            Assert.AreEqual(expItem.Name, getTodo.Data.Name, "Item Names are not the same, expected " + expItem.Name + " but got " + getTodo.Data.Name);
            Assert.AreEqual(expItem.DateDue, getTodo.Data.DateDue, "Item DateDue are not the same, expected " + expItem.DateDue + " but got " + getTodo.Data.DateDue);
            Assert.AreEqual(expItem.IsComplete, getTodo.Data.IsComplete, "Item IsComplete are not the same, expected " + expItem.IsComplete + " but got " + getTodo.Data.IsComplete);
        }

        //we need to tell NUnit where to get the test data
        [Test, TestCaseSource(typeof(TodoItemTestData), "NameBoundaryTestCases")]
        public string PostTodoItemNameBoundaryTests(string name)
        {
            //Arrange
            TodoItem postItem = new TodoItem
            {
                Name = name,
                DateDue = new DateTime(2019, 12, 31)
            };
            var postRequest = Helpers.PostTodoItemRequest(postItem);

            //Act
            IRestResponse<TodoItem> response = _client.Execute<TodoItem>(postRequest);

            //Assert
            return response.StatusCode.ToString();
        }

    }
}
