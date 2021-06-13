namespace ProductManager.Core.Exceptions
{
    public class ProductAlreadyExistsException : DomainException
    {
        public ProductAlreadyExistsException(string message) : base(message)
        {

        }
    }
}