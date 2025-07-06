using BookLibrary.Auth.Model;
using BookLibrary.Data;
using BookLibrary.Data.Entities;
using BookLibrary.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserFavoritesController : ControllerBase
    {
        private readonly BookLibraryDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserFavoritesController(BookLibraryDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserFavoriteDto>>> GetUserFavorites()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized();
            }

            var favorites = await _context.UserFavorites
                .Include(uf => uf.Book)
                .ThenInclude(b => b.Genre)
                .Where(uf => uf.UserId == userId)
                .OrderByDescending(uf => uf.CreatedAt)
                .Select(uf => new UserFavoriteDto
                {
                    Id = uf.Id,
                    UserId = uf.UserId,
                    BookId = uf.BookId,
                    CreatedAt = uf.CreatedAt,
                    Book = new BookDto
                    {
                        Id = uf.Book.Id,
                        Title = uf.Book.Title,
                        Author = uf.Book.Author,
                        ISBN = uf.Book.ISBN,
                        Description = uf.Book.Description,
                        PublicationYear = uf.Book.PublicationYear,
                        GenreId = uf.Book.GenreId,
                        GenreName = uf.Book.Genre.Name,
                        AverageRating = uf.Book.BookRatings.Any() ? uf.Book.BookRatings.Average(br => br.Rating) : null
                    }
                })
                .ToListAsync();

            return Ok(favorites);
        }

        [HttpPost]
        public async Task<ActionResult<UserFavoriteDto>> AddToFavorites([FromBody] CreateUserFavoriteDto createFavoriteDto)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized();
            }

            // Check if book exists
            var book = await _context.Books.FindAsync(createFavoriteDto.BookId);
            if (book == null)
            {
                return NotFound("Book not found");
            }

            // Check if already in favorites
            var existingFavorite = await _context.UserFavorites
                .FirstOrDefaultAsync(uf => uf.UserId == userId && uf.BookId == createFavoriteDto.BookId);

            if (existingFavorite != null)
            {
                return BadRequest("Book is already in your favorites");
            }

            var userFavorite = new UserFavorite
            {
                UserId = userId,
                BookId = createFavoriteDto.BookId,
                CreatedAt = DateTime.UtcNow
            };

            _context.UserFavorites.Add(userFavorite);
            await _context.SaveChangesAsync();

            // Return the created favorite with book details
            var createdFavorite = await _context.UserFavorites
                .Include(uf => uf.Book)
                .ThenInclude(b => b.Genre)
                .Where(uf => uf.Id == userFavorite.Id)
                .Select(uf => new UserFavoriteDto
                {
                    Id = uf.Id,
                    UserId = uf.UserId,
                    BookId = uf.BookId,
                    CreatedAt = uf.CreatedAt,
                    Book = new BookDto
                    {
                        Id = uf.Book.Id,
                        Title = uf.Book.Title,
                        Author = uf.Book.Author,
                        ISBN = uf.Book.ISBN,
                        Description = uf.Book.Description,
                        PublicationYear = uf.Book.PublicationYear,
                        GenreId = uf.Book.GenreId,
                        GenreName = uf.Book.Genre.Name,
                        AverageRating = uf.Book.BookRatings.Any() ? uf.Book.BookRatings.Average(br => br.Rating) : null
                    }
                })
                .FirstAsync();

            return CreatedAtAction(nameof(GetUserFavorites), createdFavorite);
        }

        [HttpDelete("{bookId}")]
        public async Task<IActionResult> RemoveFromFavorites(int bookId)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized();
            }

            var favorite = await _context.UserFavorites
                .FirstOrDefaultAsync(uf => uf.UserId == userId && uf.BookId == bookId);

            if (favorite == null)
            {
                return NotFound("Favorite not found");
            }

            _context.UserFavorites.Remove(favorite);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("check/{bookId}")]
        public async Task<ActionResult<bool>> IsFavorite(int bookId)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized();
            }

            var isFavorite = await _context.UserFavorites
                .AnyAsync(uf => uf.UserId == userId && uf.BookId == bookId);

            return Ok(isFavorite);
        }
    }
} 