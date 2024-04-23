using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Exceptions
{
    public class TodoItemCreationException : Exception
    {
        public TodoItemCreationException(string message) : base(message) {}
    }
}