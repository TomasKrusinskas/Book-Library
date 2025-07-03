using BookLibrary.Data.Entities;

namespace BookLibrary.Repositories.Interfaces
{
    public interface IBookRepository : IGenericRepository<Book>
    {
        Task<IEnumerable<Book>> GetBooksByGenreAsync(int genreId);
        Task<IEnumerable<Book>> GetBooksWithGenreAsync();
        Task<Book?> GetByIdWithGenreAsync(int id);
    }
}