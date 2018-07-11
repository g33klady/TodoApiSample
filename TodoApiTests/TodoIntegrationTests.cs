using NUnit.Framework;
using RestSharp;
using System;
using System.Collections;
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
            var client = new RestClient(_baseUrl);
            var request = new RestRequest(Method.GET);

            //Act
            IRestResponse response = client.Execute(request);

            //Assert
            Assert.IsTrue(response.IsSuccessful, "Get method did not return a success status code; it returned " + response.StatusCode);
        }

        [Test]
        public void VerifyGetTodoItem1ReturnsCorrectName()
        {
            //Arrange
            var expectedName = "Walk the dog"; //we know this is what it should be from the Controller constructor
            var client = new RestClient(_baseUrl);
            var request = new RestRequest("1", Method.GET); //so our URL looks like https://localhost:44350/api/Todo/1

            //Act
            IRestResponse<TodoItem> actualTodo = client.Execute<TodoItem>(request);

            //Assert
            Assert.AreEqual(expectedName, actualTodo.Data.Name, "Expected and actual names are different. Expected " + expectedName + " but was " + actualTodo.Data.Name);
        }

        [Test]
        public void VerifyGetTodoItemsReturns3Items() //this is a bad test - the count is different depending on the sequence of tests!
        {
            //Arrange
            int expectedCount = 3;
            var client = new RestClient(_baseUrl);
            var request = new RestRequest(Method.GET);

            //Act
            IRestResponse<List<TodoItem>> todoList = client.Execute<List<TodoItem>>(request); //we're doing a get of all items, so we'll have a List of TodoItem objects to deal with

            //Assert
            Assert.IsTrue(todoList.Data.Count == expectedCount, "Actual count was not " + expectedCount + " it was " + todoList.Data.Count);
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
            var client = new RestClient(_baseUrl);
            var postRequest = new RestRequest(Method.POST);
            postRequest.RequestFormat = DataFormat.Json;
            postRequest.AddJsonBody(expItem);

            //Act
            //first we need to do the POST action, and get the new object's ID
            IRestResponse<TodoItem> postTodo = client.Execute<TodoItem>(postRequest);
            var newItemId = postTodo.Data.Id; //we need the ID to do the GET request of this item

            //now we need to do the GET action, using the new object's ID
            var getRequest = new RestRequest(newItemId.ToString(), Method.GET);
            IRestResponse<TodoItem> getTodo = client.Execute<TodoItem>(getRequest);

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
            var client = new RestClient(_baseUrl);
            var postRequest = new RestRequest(Method.POST);
            postRequest.RequestFormat = DataFormat.Json;
            postRequest.AddJsonBody(postItem);

            //Act
            IRestResponse<TodoItem> response = client.Execute<TodoItem>(postRequest);

            //Assert
            return response.StatusCode.ToString();
        }

    }

    public class TodoItemTestData
    {
        public static IEnumerable NameBoundaryTestCases
        {
            get
            {
                //each item defines 1. the data to use in the test and 2. what to expect as the result
                //TestCaseData(1).Returns(2);
                yield return new TestCaseData("").Returns("BadRequest"); //0 character name
                yield return new TestCaseData("cjMJeYBj4Z").Returns("Created"); //10 character name
                yield return new TestCaseData("QhRMNNv6MNY6qW8GAWm5sFCDHbETIeP4evlKopK3HRgF9RSbUlPTFBmk78vLYwLLe5rEwEJkkbSu8m9RDlKVLvIGO1eIOdoWUm1E84dEDXcSu87mLGEdL2c0vWu7r5j6R4MvG3kUSp5e8eaGy9GoSCfBnTgrp6dx4f5XMQxuPRxj54dS20ybYkWXmnN29xbDik5vuOvmGs0SQHh9WGJvFJisWZnC8h7KZIKjy2Xp0k3de2VsNsB6jFFKsIwI18J").Returns("Created"); //255 character name
                yield return new TestCaseData("QhRMNNv6MNY6qW8GAWm5sFCDHbETIeP4evlKopK3HRgF9RSbUlPTFBmk78vLYwLLe5rEwEJkkbSu8m9RDlKVLvIGO1eIOdoWUm1E84dEDXcSu87mLGEdL2c0vWu7r5j6R4MvG3kUSp5e8eaGy9GoSCfBnTgrp6dx4f5XMQxuPRxj54dS20ybYkWXmnN29xbDik5vuOvmGs0SQHh9WGJvFJisWZnC8h7KZIKjy2Xp0k3de2VsNsB6jFFKsIwI18J1").Returns("BadRequest"); //256 character name
            }
        }
    }
}
