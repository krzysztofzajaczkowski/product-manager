namespace ProductManager.Core.Exceptions
{
    public class InvalidNetPriceException : DomainException
    {
        public InvalidNetPriceException(string message) : base(message)
        {

        }
    }
}