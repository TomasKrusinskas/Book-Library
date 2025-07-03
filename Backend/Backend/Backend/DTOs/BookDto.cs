namespace BookLibrary.DTOs
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int PublicationYear { get; set; }
        public int GenreId { get; set; }
        public string GenreName { get; set; } = string.Empty;
    }
}