namespace ErrorHandling_AspNetCore.Exceptions
{
    public class UnauthorizedException : DomainException
    {
        public UnauthorizedException(string action)
        : base("Forbidden", $"Access denied for '{action}'","FORBIDDEN")

        {

        }
    }
}
