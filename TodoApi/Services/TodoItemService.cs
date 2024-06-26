using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TodoApi.Exceptions;
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

        public async Task CreateTodoItemAsync(TodoItemDTO item)
        {
            try
            {
                var todoItem = new TodoItem
                {
                    IsComplete = item.IsComplete,
                    Name = item.Name,
                };
                await _repository.AddAsync(todoItem);
            }
            catch (Exception ex)
            {
                throw new TodoItemCreationException($"Failed to create TodoItem. {ex.Message}");
            }
        }

        public async Task DeleteTodoItemAsync(long id)
        {
            var todoItem = await _repository.GetByIdAsync(id) ?? throw new TodoItemNotFoundException(id);
            await _repository.DeleteAsync(todoItem);
        }

        public async Task<bool> ExistsAsync(long id)
        {
            return await _repository.ExistsAsync(id);
        }

        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetAllTodoItemsAsync()
        {
            try
            {
                var todoItems = await _repository.GetAllAsync();
                if(todoItems == null || !todoItems.Any())
                {
                    throw new TodoServiceException("No task items found.");
                }
                return todoItems.Select(ItemToDTO).ToList();
            }
            catch (Exception ex)
            {                
                throw new TodoServiceException("Error getting task items.", ex);
            }
        }

         public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetSearchTodoItemForName(string name)
        {
            try
            {
                IQueryable<TodoItem> query = GetQueryable();

                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(x => x.Name.Contains(name));  
                }

                var matchItens = await query
                .ToListAsync();    

                return matchItens.Select(ItemToDTO).ToList();
            }
            catch (Exception ex)
            {
                throw new TodoItemSearchException($"An error occurred while searching for TodoItems. {ex}");
            }
        }

        public IQueryable<TodoItem> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public async Task<TodoItemDTO> GetTodoItemByIdAsync(long id)
        {
            var todoItem = await _repository.GetByIdAsync(id) ?? throw new TodoItemNotFoundException(id);
            return ItemToDTO(todoItem);
        }

        public async Task<TodoServiceResult> UpdateTodoItemAsync(long id,TodoItemDTO todoDTO)
        {
            if (id != todoDTO.Id)
            {
                throw new InvalidOperationException($"The ID from the parameter ({id}) does not match the DTO's ID ({todoDTO.Id})");
            }

            var todoItem = await _repository.GetByIdAsync(id);
            if (todoItem == null)
            {
                throw new TodoItemNotFoundException(id);
            }

            todoItem.Name = todoDTO.Name;
            todoItem.IsComplete = todoDTO.IsComplete;

            try
            {
                await _repository.UpdateAsync(todoItem);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new DbUpdateConcurrencyException($"Concurrency exception when updating Todo item with ID {id}. {ex}");
            }
            catch(Exception ex)
            {
                throw new TodoItemUpdateException($"Unexpected exception when updating Todo item with ID {id}. {ex}");
            }

            return TodoServiceResult.NotFound;
        }

        public async Task<TodoServiceResult> PatchTodoItem(long id, JsonPatchDocument<TodoItemDTO> patchDocument, ModelStateDictionary modelState)
        {
            try
            {
                var todoItem = await _repository.GetByIdAsync(id);
                if (todoItem == null)
                {
                    return TodoServiceResult.NotFound;
                }

                patchDocument.ApplyTo(ItemToDTO(todoItem), modelState);

                if (!modelState.IsValid)
                {
                    return TodoServiceResult.Invalid;
                }

                try
                {
                    await _repository.UpdateAsync(todoItem);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return TodoServiceResult.NotFound;
                }

                return TodoServiceResult.Success;
            }
            catch(DbUpdateConcurrencyException)
            {
                return TodoServiceResult.NotFound;
            }
            catch (Exception ex)
            {
                throw new TodoItemPatchException($"An exception in relation to updating Patch. {ex}");
            }
        }

        private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
            new TodoItemDTO
            {
                Id = todoItem.Id,   
                Name = todoItem.Name,
                IsComplete = todoItem.IsComplete,
            };

        private static TodoItem DTOToItem(TodoItemDTO todoItem) =>
            new TodoItem
            {
                Id = todoItem.Id,
                Name = todoItem.Name,
                IsComplete = todoItem.IsComplete,
            };
    }
}