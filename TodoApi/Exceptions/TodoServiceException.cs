using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Exceptions
{
    public class TodoServiceException : Exception
    {
        public TodoServiceException() {}

        public TodoServiceException(string message) : base(message) {}

        public TodoServiceException(string message, Exception innerException) : base(message, innerException) {}
    }
}