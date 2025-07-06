namespace BookLibrary.DTOs
{
    public class UserFavoriteDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int BookId { get; set; }
        public DateTime CreatedAt { get; set; }
        public BookDto Book { get; set; } = null!;
    }

    public class CreateUserFavoriteDto
    {
        public int BookId { get; set; }
    }
} 