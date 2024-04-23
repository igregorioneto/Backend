using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using TodoApi.Models;
using TodoApi.Repositories;
using TodoApi.Services;
using TodoApi.Exceptions;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoItemService _service;

        public TodoItemsController(ITodoItemService service)
        {
            _service = service;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            return await _service.GetAllTodoItemsAsync();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetSearchTodoItems(string name)
        {
            IQueryable<TodoItem> query = _service.GetQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name.Contains(name));  
            }

            var matchItens = await query
            .Select(x => ItemToDTO(x))
            .ToListAsync();    

            return matchItens;
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
        {
            try
            {
                var todoItem = await _service.GetTodoItemByIdAsync(id);
                return Ok(todoItem);
            }
            catch (TodoItemNotFoundException)
            {
                return NotFound();
            }
        }

        // PATCH: api/TodoItems/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchTodoItem(long id, JsonPatchDocument<TodoItemDTO> patchDocument)
        {
            if (await TodoItemExists(id))
            {
                return NotFound();
            }

            var todoItem = await _service.GetTodoItemByIdAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }
            patchDocument.ApplyTo(todoItem, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _service.UpdateTodoItemAsync(todoItem);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return NoContent();
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItemDTO todoDTO)
        {
            if (id != todoDTO.Id)
            {
                return BadRequest();
            }

            if (!await TodoItemExists(id))
            {
                return NotFound();
            }

            var todoItem = await _service.GetTodoItemByIdAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.Name = todoDTO.Name;
            todoItem.IsComplete = todoDTO.IsComplete;

            try
            {
                await _service.UpdateTodoItemAsync(todoItem);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItemDTO todoDTO)
        {
            var todoItem = new TodoItem
            {
                IsComplete = todoDTO.IsComplete,
                Name = todoDTO.Name,
            };

            await _service.CreateTodoItemAsync(todoItem);

            // return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
            return CreatedAtAction(
                nameof(GetTodoItem), 
                new { id = todoItem.Id }, 
                todoItem);
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _service.GetTodoItemByIdAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            await _service.DeleteTodoItemAsync(todoItem);

            return NoContent();
        }

        private async Task<bool> TodoItemExists(long id)
        {
            return await _service.ExistsAsync(id);
        }

        private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
            new TodoItemDTO
            {
                Id = todoItem.Id,
                Name = todoItem.Name,
                IsComplete = todoItem.IsComplete,
            };
    }
}
