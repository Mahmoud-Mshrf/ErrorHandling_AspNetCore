using System.ComponentModel.DataAnnotations;

namespace ErrorHandling_AspNetCore.Dtos
{
    public class AddUserDto
    {
        [Required]
        public string FullName { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
