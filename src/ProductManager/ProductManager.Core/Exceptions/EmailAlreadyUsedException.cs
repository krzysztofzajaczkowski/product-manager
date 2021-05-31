using System;

namespace ProductManager.Core.Exceptions
{
    public class EmailAlreadyUsedException : DomainException
    {
        public EmailAlreadyUsedException(string message) : base(message)
        {

        }
    }
}