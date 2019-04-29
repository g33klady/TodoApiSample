using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TodoApiUnitTests
{
    [TestClass]
    public class UtilsUnitTests
    {
        [TestMethod]
        public void IsItemNameValidReturnsFalseWhenItemNameIsEmptyString()
        {
            TodoApi.Models.TodoItem item = new TodoApi.Models.TodoItem();
            item.Name = "";

            var result = TodoApi.Utils.IsItemNameValid(item);

            Assert.IsFalse(result);

        }

        [TestMethod]
        public void IsDateValidReturnsFalseWhenDateDueIsMinValue()
        {
            TodoApi.Models.TodoItem item = new TodoApi.Models.TodoItem();
            item.Name = "test";
            item.DateDue = System.DateTime.MinValue;

            var result = TodoApi.Utils.IsItemDateValid(item);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDateValidReturnsFalseWhenDateDueIsPastDate()
        {
            TodoApi.Models.TodoItem item = new TodoApi.Models.TodoItem();
            item.Name = "test";
            item.DateDue = new System.DateTime(2018, 01, 01);

            var result = TodoApi.Utils.IsItemDateValid(item);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDateValidReturnsTrueWhenDateDueIsToday()
        {
            TodoApi.Models.TodoItem item = new TodoApi.Models.TodoItem();
            item.Name = "test";
            item.DateDue = System.DateTime.Today;

            var result = TodoApi.Utils.IsItemDateValid(item);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsDateValidReturnsTrueWhenDateDueIsFutureDate()
        {
            TodoApi.Models.TodoItem item = new TodoApi.Models.TodoItem();
            item.Name = "test";
            item.DateDue = System.DateTime.MaxValue;

            var result = TodoApi.Utils.IsItemDateValid(item);
            Assert.IsTrue(result);
        }
    }

    
}
