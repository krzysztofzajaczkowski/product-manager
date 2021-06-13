namespace ProductManager.Core.Exceptions
{
    public class InvalidDescriptionException : DomainException
    {
        public InvalidDescriptionException(string message) : base(message)
        {

        }
    }
}