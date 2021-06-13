namespace ProductManager.Core.Exceptions
{
    public class InvalidProductNameException : DomainException
    {
        public InvalidProductNameException(string message) : base(message)
        {

        }
    }
}