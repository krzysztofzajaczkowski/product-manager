namespace ProductManager.Core.Exceptions
{
    public class InvalidSkuException : DomainException
    {
        public InvalidSkuException(string message) : base(message)
        {

        }
    }
}