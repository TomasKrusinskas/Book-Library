using BookLibrary.Data.Entities;
using BookLibrary.DTOs;
using BookLibrary.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
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
                GenreName = b.Genre?.Name ?? ""
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
                GenreName = book.Genre?.Name ?? ""
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
                GenreName = b.Genre?.Name ?? ""
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
                GenreId = createBookDto.GenreId
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
                GenreId = book.GenreId
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
    }
}