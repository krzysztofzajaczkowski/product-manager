using System;

namespace ProductManager.Core.Exceptions
{
    public class RoleNotFoundException : DomainException
    {
        public RoleNotFoundException(string message) : base(message)
        {
            
        }
    }
}