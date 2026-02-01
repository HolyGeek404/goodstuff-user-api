using GoodStuff.UserApi.Domain.Entities;
using GoodStuff.UserApi.Domain.ValueObjects;

namespace GoodStuff.UserApi.Application.Services.Interfaces;

public interface IUserRepository
{
    Task SignUpAsync(User user);
    Task<User?> GetUserByEmailAsync(Email email);
    Task<bool> ActivateUserAsync(Email email, ActivationToken providedKey);
    Task RemoveUserAsync(Email user);
}