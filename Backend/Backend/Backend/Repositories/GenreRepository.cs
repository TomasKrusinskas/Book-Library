using BookLibrary.Data;
using BookLibrary.Data.Entities;
using BookLibrary.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Repositories
{
    public class GenreRepository : GenericRepository<Genre>, IGenreRepository
    {
        public GenreRepository(BookLibraryDbContext context) : base(context)
        {
        }

        public async Task<Genre?> GetGenreWithBooksAsync(int id)
        {
            return await _context.Genres
                .Include(g => g.Books)
                .FirstOrDefaultAsync(g => g.Id == id);
        }
    }
}