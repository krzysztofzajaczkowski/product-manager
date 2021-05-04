using System;

namespace ProductManager.Core.Exceptions
{
    public class RoleNotFoundException : Exception
    {
        public RoleNotFoundException(string message) : base(message)
        {
            
        }
    }
}