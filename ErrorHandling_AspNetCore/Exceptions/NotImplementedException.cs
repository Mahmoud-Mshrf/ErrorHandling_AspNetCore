namespace ErrorHandling_AspNetCore.Exceptions
{
    public class NotImplementedException : DomainException
    {
        public NotImplementedException(string msg) : base("Not_Implemented",msg,"NOT_IMPLEMENTED")
        {

        }
    }
}
