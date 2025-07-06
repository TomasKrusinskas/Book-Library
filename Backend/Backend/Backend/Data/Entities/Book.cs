using System.Collections.Generic;

namespace BookLibrary.Data.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int PublicationYear { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; } = null!;
        public string Summary { get; set; } = string.Empty;
        public ICollection<BookRating> BookRatings { get; set; } = new List<BookRating>();
    }
}