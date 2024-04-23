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
        Task<ActionResult<IEnumerable<TodoItem>>> GetAllTodoItemsAsync();
        Task<TodoItem> GetTodoItemByIdAsync(long id);
        Task CreateTodoItemAsync(TodoItem item);
        Task UpdateTodoItemAsync(TodoItem item);
        Task DeleteTodoItemAsync(TodoItem item);
        Task<bool> ExistsAsync(long id);
        IQueryable<TodoItem> GetQueryable();
    }
}