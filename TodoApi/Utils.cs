using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi
{
    public static class Utils
    {
        public static bool IsItemNameValid(TodoItem item)
        {
            var name = item.Name;
            if (name == null || name.Length > 255 || name.Length < 1)
            {
                return false;
            }
            return true;
        }

        public static bool IsItemDateValid(TodoItem item)
        {
            var date = item.DateDue;
            if (date == DateTime.MinValue)
            {
                return true;
            }
            DateTime today = DateTime.Today;
            var diff = DateTime.Compare(date, today);
            if (diff < 0)
            {
                return false; //item.datedue is in the past
            }
            else
            {
                return true; //item.datedue is today or in the future
            }
        }

        public static void InsertBaseTodoItems(TodoContext context)
        {
            //some base items to have in our todo list on launch so it's not empty
            context.TodoItems.Add(new TodoItem { Name = "Walk the dog", DateDue = new DateTime(2019, 12, 31) });
            context.TodoItems.Add(new TodoItem { Name = "Feed the dog", DateDue = new DateTime(2019, 12, 30) });
            context.TodoItems.Add(new TodoItem { Name = "Walk the cat", DateDue = new DateTime(2019, 12, 29) });
            context.SaveChanges();
        }
    }
}
