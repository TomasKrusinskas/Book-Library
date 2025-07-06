using System;
using BookLibrary.Auth.Model;

namespace BookLibrary.Data.Entities
{
    public class BookRating
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; } = null!;
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;
        public int Rating { get; set; } // 1-5
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
} 