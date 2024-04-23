using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Repositories;

namespace TodoApi.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ITodoItemRepository _repository;

        public TodoItemService(ITodoItemRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateTodoItemAsync(TodoItem item)
        {
            await _repository.AddAsync(item);
        }

        public async Task DeleteTodoItemAsync(TodoItem item)
        {
            await _repository.DeleteAsync(item);
        }

        public async Task<bool> ExistsAsync(long id)
        {
            return await _repository.ExistsAsync(id);
        }

        public async Task<ActionResult<IEnumerable<TodoItem>>> GetAllTodoItemsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public IQueryable<TodoItem> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public async Task<TodoItem> GetTodoItemByIdAsync(long id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task UpdateTodoItemAsync(TodoItem item)
        {
            await _repository.UpdateAsync(item);
        }
    }
}