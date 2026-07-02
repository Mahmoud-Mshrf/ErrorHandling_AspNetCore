using System.ComponentModel.DataAnnotations;

namespace ErrorHandling_AspNetCore.Models
{
    public class Driver
    {
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; }
        public int DriverNumber { get; set; }
    }
}
