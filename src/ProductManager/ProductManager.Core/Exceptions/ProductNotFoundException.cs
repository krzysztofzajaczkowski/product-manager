namespace ProductManager.Core.Exceptions
{
    public class ProductNotFoundException : DomainException
    {
        public ProductNotFoundException(string message) : base(message)
        {
        }
    }
}