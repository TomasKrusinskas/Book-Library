using System.ComponentModel.DataAnnotations;

namespace BookLibrary.DTOs
{
    public class CreateBookDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Author { get; set; } = string.Empty;

        [Required]
        public string ISBN { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required]
        public int PublicationYear { get; set; }

        [Required]
        public int GenreId { get; set; }

        public string Summary { get; set; } = string.Empty;
    }
}