using System;

namespace ProductManager.Core.Exceptions
{
    public class InvalidCredentialsException : DomainException
    {
        public InvalidCredentialsException(string message) : base(message)
        {

        }
    }
}