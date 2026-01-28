using GoodStuff.UserApi.Domain.Entities;

namespace GoodStuff.UserApi.Application.Services.Interfaces;

public interface IUserRepository
{
    Task SignUpAsync(User user);
    Task<User?> GetUserByEmailAsync(string email);
    Task<bool> ActivateUserAsync(string email, Guid providedKey);
}