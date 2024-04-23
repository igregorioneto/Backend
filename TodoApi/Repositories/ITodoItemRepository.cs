using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;

namespace TodoApi.Repositories
{
    public interface ITodoItemRepository
    {
        Task<IEnumerable<TodoItem>> GetAllAsync();
        Task<TodoItem> GetByIdAsync(long id);
        Task AddAsync(TodoItem item);
        Task UpdateAsync(TodoItem item);
        Task DeleteAsync(TodoItem item);   
        Task<bool> ExistsAsync(long id);
        IQueryable<TodoItem> GetQueryable();
    }
}