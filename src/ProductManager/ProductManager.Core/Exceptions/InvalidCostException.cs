namespace ProductManager.Core.Exceptions
{
    public class InvalidCostException : DomainException
    {
        public InvalidCostException(string message) : base(message)
        {

        }
    }
}