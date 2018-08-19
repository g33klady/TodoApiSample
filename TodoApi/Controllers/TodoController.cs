using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
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
        /// <summary>
        /// Gets a List of all TodoItems
        /// </summary>
        /// <returns>All TodoItems</returns>
        /// <response code="200">Returns a list of all TodoItems</response>
        /// <response code="401">If missing authorization header</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<TodoItem>), 200)]
        [ProducesResponseType(401)]
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

        /// <summary>
        /// Gets a single TodoItem by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Single ToDo Item</returns>
        /// <response code="200">Returns a single TodoItem</response>
        /// <response code="401">If missing authorization header</response>
        /// <response code="404">If TodoItem ID is not found</response>
        [HttpGet("{id}", Name = "GetTodo")]
        [ProducesResponseType(typeof(TodoItem), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
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

        /// <summary>
        /// Creates a single TodoItem
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /Todo
        ///     {
        ///         "name": "item1",
        ///         "isComplete": false,
        ///         "dateDue": "12/31/2019"
        ///     }
        /// </remarks>
        /// <param name="item"></param>
        /// <returns>A newly created TodoItem</returns>
        /// <response code="201">Returns the newly created TodoItem</response>
        /// <response code="400">If the item is not correctly formed</response>
        /// <response code="401">If missing authorization header</response>
        [HttpPost]
        [ProducesResponseType(typeof(TodoItem), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
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

        /// <summary>
        /// Updates a single TodoItem; provide ALL fields
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /Todo/1
        ///     {
        ///         "name": "item1",
        ///         "isComplete": true,
        ///         "dateDue": "12/31/2019"
        ///     }
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns>The updated TodoItem</returns>
        /// <response code="201">Returns the updated TodoItem</response>
        /// <response code="400">If the item is not correctly formed</response>
        /// <response code="401">If missing authorization header</response>
        /// <response code="404">If TodoItem ID is not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
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

        /// <summary>
        /// Deletes a single TodoItem by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="204">If Delete was successful</response>
        /// <response code="401">If missing authorization header</response>
        /// <response code="404">If TodoItem ID is not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
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