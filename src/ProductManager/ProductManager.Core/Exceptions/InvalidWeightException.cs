namespace ProductManager.Core.Exceptions
{
    public class InvalidWeightException : DomainException
    {
        public InvalidWeightException(string message) : base(message)
        {

        }
    }
}