using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManager.Core.Exceptions
{
    public class InvalidRoleNameException : DomainException
    {
        public InvalidRoleNameException(string message): base(message)
        {
            
        }
    }
}
