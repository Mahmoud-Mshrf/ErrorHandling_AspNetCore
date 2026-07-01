namespace ErrorHandling_AspNetCore.Exceptions
{
    public class DomainException : Exception
    {
        public string ErrorCode { get; }
        public string Title { get; }
        public DomainException(string title, string msg, string errorCode) : base(msg)
        {
            ErrorCode = errorCode;
            Title = title;
        }
    }
}
