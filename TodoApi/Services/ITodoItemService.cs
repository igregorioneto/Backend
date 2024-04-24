using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
    }
}