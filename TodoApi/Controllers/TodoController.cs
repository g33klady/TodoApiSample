using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;

            //if (_context.TodoItems.Count() == 0)
            //{
            //    _context.TodoItems.Add(new TodoItem { Name = "Item1" });
            //    _context.SaveChanges();
            //}
        }

        [HttpGet]
        public ActionResult<List<TodoItem>> GetAll()
        {
            return _context.TodoItems.ToList();
        }

        //[HttpGet("limit")]
        //public ActionResult<List<TodoItem>> GetSome(int qty)
        //{
        //    var fullList = _context.TodoItems.ToList();
        //    var returnList = new List<TodoItem>();
        //    for (int i = 0; i < qty; i++)
        //    {
        //        returnList.Add(fullList[i]);
        //    }
        //    return returnList;
        //}

        [HttpGet("{id}", Name = "GetTodo")]
        public ActionResult<TodoItem> GetById(long id)
        {
            var item = _context.TodoItems.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        [HttpPost]
        public IActionResult Create([FromBody]TodoItem item)
        {
            if(!IsItemNameValid(item))
            {
                return BadRequest("Name is required and must be 1-255 chars long.");
            }
            if(!IsItemDateValid(item))
            {
                return BadRequest("Date must be valid and in the future.");
            }
            _context.TodoItems.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody]TodoItem item)
        {
            var todo = _context.TodoItems.Find(id);
            if (todo == null)
            {
                return NotFound();
            }
            if (todo.Name == "")
            {
                return BadRequest("Name is required.");
            }
            todo.IsComplete = item.IsComplete;
            todo.Name = item.Name;
            todo.DateDue = item.DateDue;

            _context.TodoItems.Update(todo);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var todo = _context.TodoItems.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todo);
            _context.SaveChanges();
            return NoContent();
        }

        public bool IsItemNameValid(TodoItem item)
        {
            var name = item.Name;
            if(name == null || name.Length > 255 || name.Length < 1)
            {
                return false;
            }
            return true;
        }

        public bool IsItemDateValid(TodoItem item)
        {
           var date = item.DateDue;
           if (date == DateTime.MinValue)
            {
                return true;
            }
           DateTime today = DateTime.Today;
           var diff = DateTime.Compare(date, today);
           if(diff < 0)
            {
                return false; //item.datedue is in the past
            }
           else
            {
                return true; //item.datedue is today or in the future
            }
        }
    }
}