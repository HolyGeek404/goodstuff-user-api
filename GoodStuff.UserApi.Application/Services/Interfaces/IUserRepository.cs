using GoodStuff.UserApi.Domain.Models.User;

namespace GoodStuff.UserApi.Application.Services.Interfaces;

public interface IUserRepository
{
    Task SignUpAsync(Users user);
    Task<Users?> GetUserByEmailAsync(string email);
    Task<bool> ActivateUserAsync(string email, Guid providedKey);
}