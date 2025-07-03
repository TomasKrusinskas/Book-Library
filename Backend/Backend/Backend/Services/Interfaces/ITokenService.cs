using BookLibrary.Auth.Model;

namespace BookLibrary.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(ApplicationUser user);
    }
}