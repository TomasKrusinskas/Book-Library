using BookLibrary.Data;
using BookLibrary.Data.Entities;
using BookLibrary.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Repositories
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        public BookRepository(BookLibraryDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Book>> GetBooksByGenreAsync(int genreId)
        {
            return await _context.Books
                .Where(b => b.GenreId == genreId)
                .Include(b => b.Genre)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksWithGenreAsync()
        {
            return await _context.Books
                .Include(b => b.Genre)
                .ToListAsync();
        }

        public async Task<Book?> GetByIdWithGenreAsync(int id)
        {
            return await _context.Books
                .Include(b => b.Genre)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}
