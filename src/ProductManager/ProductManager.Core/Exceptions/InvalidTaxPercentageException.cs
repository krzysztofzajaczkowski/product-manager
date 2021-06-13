namespace ProductManager.Core.Exceptions
{
    public class InvalidTaxPercentageException : DomainException
    {
        public InvalidTaxPercentageException(string message) : base(message)
        {

        }
    }
}