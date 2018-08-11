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
            //comment this out if you don't want the default todo items populating on launch
            if (_context.TodoItems.Count() == 0)
            {
                Utils.InsertBaseTodoItems(_context);
            }
        }

        [HttpGet]
        public ActionResult<List<TodoItem>> GetAll()
        {
            if (!Utils.CanAccess(Request.Headers))
            {
                return Unauthorized();
            }
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
            if (!Utils.CanAccess(Request.Headers))
            {
                return Unauthorized();
            }
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
            if (!Utils.CanAccess(Request.Headers))
            {
                return Unauthorized();
            }
            if (!Utils.IsItemNameValid(item))
            {
                return BadRequest("Name is required and must be 1-255 chars long.");
            }
            if (item.DateDue != null)
            {
                if (!Utils.IsItemDateValid(item))
                {
                    return BadRequest("Date must be valid and in the future.");
                }
                item.DateDue = item.DateDue.Date;
            }

            _context.TodoItems.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody]TodoItem item)
        {
            if (!Utils.CanAccess(Request.Headers))
            {
                return Unauthorized();
            }
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
            if (!Utils.CanAccess(Request.Headers))
            {
                return Unauthorized();
            }
            var todo = _context.TodoItems.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todo);
            _context.SaveChanges();
            return NoContent();
        }
    }
}