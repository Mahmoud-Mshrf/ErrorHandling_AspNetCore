namespace ErrorHandling_AspNetCore.Dtos
{
    public class ResultDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string[] Errors { get; set; }
    }
}
