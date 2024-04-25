using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Exceptions
{
    public class TodoItemSearchException : Exception
    {
        public TodoItemSearchException(string message) : base(message) {}
    }
}