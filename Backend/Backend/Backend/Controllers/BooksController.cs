using BookLibrary.Data;
using BookLibrary.Data.Entities;
using BookLibrary.DTOs;
using BookLibrary.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly BookLibraryDbContext _context;

        public BooksController(IBookRepository bookRepository, BookLibraryDbContext context)
        {
            _bookRepository = bookRepository;
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
        {
            var books = await _bookRepository.GetBooksWithGenreAsync();
            var bookDtos = books.Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                ISBN = b.ISBN,
                Description = b.Description,
                PublicationYear = b.PublicationYear,
                GenreId = b.GenreId,
                GenreName = b.Genre?.Name ?? "",
                Summary = b.Description?.Length > 120 ? b.Description.Substring(0, 120) + "..." : b.Description,
                PhotoUrl = b.PhotoUrl,
                Price = b.Price,
                AverageRating = _bookRepository.GetAverageRating(b.Id)
            });

            return Ok(bookDtos);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<BookDto>> GetBook(int id)
        {
            var book = await _bookRepository.GetByIdWithGenreAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                Description = book.Description,
                PublicationYear = book.PublicationYear,
                GenreId = book.GenreId,
                GenreName = book.Genre?.Name ?? "",
                Summary = book.Description?.Length > 120 ? book.Description.Substring(0, 120) + "..." : book.Description,
                PhotoUrl = book.PhotoUrl,
                Price = book.Price,
                AverageRating = _bookRepository.GetAverageRating(book.Id)
            });
        }

        [HttpGet("genre/{genreId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooksByGenre(int genreId)
        {
            var books = await _bookRepository.GetBooksByGenreAsync(genreId);
            var bookDtos = books.Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                ISBN = b.ISBN,
                Description = b.Description,
                PublicationYear = b.PublicationYear,
                GenreId = b.GenreId,
                GenreName = b.Genre?.Name ?? "",
                Summary = b.Description?.Length > 120 ? b.Description.Substring(0, 120) + "..." : b.Description,
                PhotoUrl = b.PhotoUrl,
                Price = b.Price,
                AverageRating = _bookRepository.GetAverageRating(b.Id)
            });

            return Ok(bookDtos);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BookDto>> CreateBook([FromBody] CreateBookDto createBookDto)
        {
            var book = new Book
            {
                Title = createBookDto.Title,
                Author = createBookDto.Author,
                ISBN = createBookDto.ISBN,
                Description = createBookDto.Description,
                PublicationYear = createBookDto.PublicationYear,
                GenreId = createBookDto.GenreId,
                Price = createBookDto.Price,
                // Add Summary if needed
                // Summary = createBookDto.Summary
            };

            await _bookRepository.AddAsync(book);
            await _bookRepository.SaveAsync();

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                Description = book.Description,
                PublicationYear = book.PublicationYear,
                GenreId = book.GenreId,
                GenreName = book.Genre?.Name ?? "",
                Summary = book.Description?.Length > 120 ? book.Description.Substring(0, 120) + "..." : book.Description,
                PhotoUrl = book.PhotoUrl,
                Price = book.Price,
                AverageRating = 0
            });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] CreateBookDto updateBookDto)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            book.Title = updateBookDto.Title;
            book.Author = updateBookDto.Author;
            book.ISBN = updateBookDto.ISBN;
            book.Description = updateBookDto.Description;
            book.PublicationYear = updateBookDto.PublicationYear;
            book.GenreId = updateBookDto.GenreId;
            book.Price = updateBookDto.Price;
            // book.Summary = updateBookDto.Summary;

            _bookRepository.Update(book);
            await _bookRepository.SaveAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _bookRepository.Delete(book);
            await _bookRepository.SaveAsync();

            return NoContent();
        }

        [HttpPost("{id}/rate")]
        [Authorize]
        public async Task<IActionResult> RateBook(int id, [FromBody] int rating)
        {
            if (rating < 1 || rating > 5)
                return BadRequest("Rating must be between 1 and 5.");

            var userId = User.FindFirst("sub")?.Value ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var existing = await _context.BookRatings.FirstOrDefaultAsync(r => r.BookId == id && r.UserId == userId);
            if (existing != null)
            {
                existing.Rating = rating;
                existing.CreatedAt = DateTime.UtcNow;
                _context.BookRatings.Update(existing);
            }
            else
            {
                _context.BookRatings.Add(new BookRating
                {
                    BookId = id,
                    UserId = userId,
                    Rating = rating,
                    CreatedAt = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("{id}/my-rating")]
        [Authorize]
        public async Task<ActionResult<int?>> GetMyRating(int id)
        {
            var userId = User.FindFirst("sub")?.Value ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var rating = await _context.BookRatings
                .Where(r => r.BookId == id && r.UserId == userId)
                .Select(r => r.Rating)
                .FirstOrDefaultAsync();

            return Ok(rating);
        }
    }
}