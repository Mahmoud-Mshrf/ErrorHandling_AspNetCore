namespace ErrorHandling_AspNetCore.Exceptions
{
    public class NotFoundException : DomainException
    {
        public NotFoundException(string name, object key) :
            base("Not Found", $"{name} with key : {key} was not found", "NOT_FOUND")
        {

        }
    }
}
