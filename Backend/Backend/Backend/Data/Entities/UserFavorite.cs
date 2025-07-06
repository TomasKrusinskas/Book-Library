using BookLibrary.Auth.Model;
using System.ComponentModel.DataAnnotations;

namespace BookLibrary.Data.Entities
{
    public class UserFavorite
    {
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        [Required]
        public int BookId { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public ApplicationUser User { get; set; } = null!;
        public Book Book { get; set; } = null!;
    }
} 