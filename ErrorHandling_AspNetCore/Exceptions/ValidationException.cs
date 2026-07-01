namespace ErrorHandling_AspNetCore.Exceptions
{
    public class ValidationException : DomainException
    {
        public IDictionary<string, string[]> Errors { get; }
        public ValidationException(IDictionary<string, string[]> errors)
            : base("Validation Error", "One or more validation errors occurred.", "VALIDATION")
            => Errors = errors;
    }

}
