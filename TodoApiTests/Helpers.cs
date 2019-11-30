using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TodoApi.Models;

namespace TodoApiTests
{
    public static class Helpers
    {
        public static RestRequest GetAllTodoItemsRequest()
        {
            var request = new RestRequest(Method.GET);
            //request.AddHeader("CanAccess", "true"); //here's where you would might a header to the request
            return request;
        }

        public static RestRequest GetSingleTodoItemRequest(long id)
        {
            var request = new RestRequest($"{id}", Method.GET);
            request.AddUrlSegment("id", id);
            return request;
        }

        public static RestRequest PostTodoItemRequest(TodoItem item)
        {
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(item);
            return request;
        }

        public static RestRequest PutTodoItemRequest(int id)
        {
            var request = new RestRequest($"{id}", Method.PUT);
            request.AddUrlSegment("id", id);
            return request;
        }

        public static TodoItem GetTestTodoItem(string name = "mow the lawn", bool isCompleted = false, DateTime dateDue = default(DateTime))
        {
            if (dateDue == default(DateTime))
            {
                dateDue = new DateTime(2029, 12, 31);
            }
            return new TodoItem
            {
                Name = name,
                DateDue = dateDue,
                IsComplete = isCompleted
            };
        }
    }
}



