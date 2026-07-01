namespace ErrorHandling_AspNetCore.Exceptions
{
    public class BadRequestException:DomainException
    {
        public BadRequestException(string msg):base("Bad Request",msg,"BAD_REQUEST")
        {
            
        }
    }

}
