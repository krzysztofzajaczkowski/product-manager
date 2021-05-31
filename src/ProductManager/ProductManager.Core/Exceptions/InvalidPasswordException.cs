using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManager.Core.Exceptions
{
    public class InvalidPasswordException : DomainException
    {
        public InvalidPasswordException(string message) : base(message)
        {

        }
    }
}
