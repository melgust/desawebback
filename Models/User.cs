using System.ComponentModel.DataAnnotations;

namespace HelloApi.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public required string Username { get; set; }

        [Required]
        public required string Password { get; set; }

        public int RoleId { get; set; }
        public Role? Role { get; set; }
    }
}
