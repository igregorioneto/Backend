using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TodoApi.Models;

namespace TodoApi.Services
{
    public interface ITodoItemService
    {
        Task<ActionResult<IEnumerable<TodoItemDTO>>> GetAllTodoItemsAsync();
        Task<TodoItemDTO> GetTodoItemByIdAsync(long id);
        Task CreateTodoItemAsync(TodoItemDTO item);
        Task UpdateTodoItemAsync(TodoItemDTO item);
        Task DeleteTodoItemAsync(long id);
        Task<bool> ExistsAsync(long id);
        IQueryable<TodoItem> GetQueryable();
        Task<ActionResult<IEnumerable<TodoItemDTO>>> GetSearchTodoItemForName(string name);
        Task<TodoServiceResult> PatchTodoItem(long id, JsonPatchDocument<TodoItemDTO> patchDocument, ModelStateDictionary modelState);
    }
}