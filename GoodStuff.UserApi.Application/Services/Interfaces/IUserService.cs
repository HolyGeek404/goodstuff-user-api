using GoodStuff.UserApi.Domain.Entities;

namespace GoodStuff.UserApi.Application.Services.Interfaces;

public interface IUserService
{
    Task<bool> SignUpAsync(User model);
    Task<User?> SignInAsync(string email, string password);
    Task<User?> GetUserByEmailAsync(string email);
    Task<bool> ActivateUserAsync(string email, Guid providedKey);
}