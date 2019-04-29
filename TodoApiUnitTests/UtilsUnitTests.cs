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
    }
}
