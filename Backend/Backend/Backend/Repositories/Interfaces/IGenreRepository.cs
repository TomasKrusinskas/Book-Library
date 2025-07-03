using BookLibrary.Data.Entities;

namespace BookLibrary.Repositories.Interfaces
{
    public interface IGenreRepository : IGenericRepository<Genre>
    {
        Task<Genre?> GetGenreWithBooksAsync(int id);
    }
}