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
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
        {
            try
            {
                var todoItemsDTO = await _service.GetAllTodoItemsAsync();
                return Ok(todoItemsDTO);
            }
            catch (TodoServiceException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new 
                {
                    Error = "Error getting task items.",
                    Message = ex.Message,
                });
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetSearchTodoItems(string name)
        {
            var matchItens = await _service.GetSearchTodoItemForName(name);  

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
            var result = await _service.PatchTodoItem(id, patchDocument, ModelState);

            if (result == TodoServiceResult.NotFound)
            {
                return NotFound();
            }

            if (result == TodoServiceResult.Invalid)
            {
                return BadRequest(ModelState);
            }

            return NoContent();
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItemDTO todoDTO)
        {
            var result = await _service.UpdateTodoItemAsync(id, todoDTO);

            if (result == TodoServiceResult.NotFound)
            {
                return NotFound();
            }

            if (result == TodoServiceResult.Invalid)
            {
                return BadRequest();
            }

            return NotFound();
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItemDTO todoDTO)
        {
            try
            {
                await _service.CreateTodoItemAsync(todoDTO);

                return CreatedAtAction(
                    nameof(GetTodoItem), 
                    new { id = todoDTO.Id }, 
                    todoDTO);
                }
            catch (TodoItemCreationException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            try
            {
                await _service.DeleteTodoItemAsync(id);

                return NoContent();
            }
            catch (TodoItemNotFoundException)
            {
                return NotFound();
            }
        }

        private async Task<bool> TodoItemExists(long id)
        {
            return await _service.ExistsAsync(id);
        }
    }
}
