using ApiTest.Context;
using ApiTest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTest.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : Controller
    {
        private TodoContext _todoContext;

        public TodoController(TodoContext todoContext)
        {
            _todoContext = todoContext;
        }

        [HttpGet]
        public IEnumerable<TodoItem> GetTodoItems()
        {
            return _todoContext.TodoEntries;
        }

        [HttpGet("{id}")]
        public TodoItem getTodoItem(int id)
        {

            return _todoContext.TodoEntries.FirstOrDefault(t => t.Id == id);
        }

        // POST api/<EmployeeController>
        [HttpPost]
        public void Post([FromBody] TodoItem value)
        {
            _todoContext.TodoEntries.Add(value);
            _todoContext.SaveChanges();
        }

        // PUT api/<EmployeeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] TodoItem value)
        {
            var todoItem = _todoContext.TodoEntries.FirstOrDefault(t => t.Id == id);
            if (todoItem != null)
            {
                _todoContext.Entry<TodoItem>(todoItem).CurrentValues.SetValues(value);
                _todoContext.SaveChanges();
            }
        }

        // DELETE api/<EmployeeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var todoItem = _todoContext.TodoEntries.FirstOrDefault(t => t.Id == id);
            if (todoItem != null)
            {
                _todoContext.TodoEntries.Remove(todoItem);
                _todoContext.SaveChanges();
            }
        }
    }
}
