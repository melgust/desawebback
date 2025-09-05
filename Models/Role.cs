using System.ComponentModel.DataAnnotations;

namespace HelloApi.Models
{
    public class Role
    {
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        public ICollection<User> Users { get; set; } = [];
    
    }
}
