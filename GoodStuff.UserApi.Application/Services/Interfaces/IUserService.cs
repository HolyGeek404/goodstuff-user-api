using GoodStuff.UserApi.Domain.Entities;
using GoodStuff.UserApi.Domain.ValueObjects;

namespace GoodStuff.UserApi.Application.Services.Interfaces;

public interface IUserService
{
    Task<bool> SignUpAsync(User model);
    Task<string?> SignInAsync(Email email, Password password);
    Task<User?> GetUserByEmailAsync(Email email);
    Task<bool> ActivateUserAsync(Email email, ActivationToken providedKey);
    Task RemoveUserAsync(Email email);
}
