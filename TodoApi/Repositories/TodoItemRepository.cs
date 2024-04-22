using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Repositories
{
    public class TodoItemRepository : ITodoItemRepository
    {
        private readonly TodoContext _context;

        public TodoItemRepository(TodoContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<TodoItem>>> GetAllAsync()
        {
            return await _context.TodoItems.ToListAsync();
        }

        public async Task<TodoItem> GetByIdAsync(long id)
        {
            return await _context.TodoItems.FindAsync(id);
        }

        public async Task AddAsync(TodoItem item)
        {
            _context.TodoItems.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TodoItem item)
        {
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TodoItem item)
        {
            _context.TodoItems.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(long id)
        {
            return await _context.TodoItems.AnyAsync(e => e.Id == id);
        }

        public IQueryable<TodoItem> GetQueryable()
        {
            return _context.TodoItems;
        }
    }
}