using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Exceptions
{
    public class TodoItemPatchException : Exception
    {
        public TodoItemPatchException(string message) : base(message) {}
    }
}