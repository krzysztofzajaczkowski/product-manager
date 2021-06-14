using System;

namespace ProductManager.Core.Exceptions
{
    public class RoleNotFoundException : NotFoundException
    {
        public RoleNotFoundException(string message) : base(message)
        {
            
        }
    }
}