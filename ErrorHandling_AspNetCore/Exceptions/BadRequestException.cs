namespace ErrorHandling_AspNetCore.Exceptions
{
    public class BadRequestException:Exception
    {
        public BadRequestException(string msg):base(msg)
        {
            
        }
    }
}
