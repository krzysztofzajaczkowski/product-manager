using System;

namespace ProductManager.Core.Exceptions
{
    public class EmailAlreadyUsedException : Exception
    {
        public EmailAlreadyUsedException(string message) : base(message)
        {

        }
    }
}