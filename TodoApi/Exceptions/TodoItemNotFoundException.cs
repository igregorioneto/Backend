using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Exceptions
{
    public class TodoItemNotFoundException : Exception
    {
        public TodoItemNotFoundException(long id) 
        : base($"Todo item with ID {id} not found.")
        {}        
    }
}