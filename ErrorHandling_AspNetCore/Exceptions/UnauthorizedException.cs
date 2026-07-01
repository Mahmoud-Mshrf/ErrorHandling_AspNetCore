namespace ErrorHandling_AspNetCore.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string msg) : base(msg)
        {

        }
    }
}
