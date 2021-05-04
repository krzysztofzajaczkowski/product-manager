using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManager.Core.Exceptions
{
    public class InvalidEmailException : Exception
    {
        public InvalidEmailException(string message) : base(message)
        {
            
        }
    }
}
