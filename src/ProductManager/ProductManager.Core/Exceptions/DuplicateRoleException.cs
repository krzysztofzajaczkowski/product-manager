using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManager.Core.Exceptions
{
    public class DuplicateRoleException : Exception
    {
        public DuplicateRoleException(string message): base(message)
        {
            
        }
    }
}
