using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManager.Core.Exceptions
{
    public class InvalidUsernameException : Exception
    {
        public InvalidUsernameException(string message) : base(message)
        {

        }
    }
}
