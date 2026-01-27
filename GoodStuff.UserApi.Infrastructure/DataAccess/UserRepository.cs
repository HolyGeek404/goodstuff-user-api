using GoodStuff.UserApi.Application.Services;
using GoodStuff.UserApi.Application.Services.Interfaces;
using GoodStuff.UserApi.Domain.Models.User;
using GoodStuff.UserApi.Infrastructure.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GoodStuff.UserApi.Infrastructure.DataAccess;

public class UserRepository(GoodStuffContext context, ILogger<UserRepository> logger) : IUserRepository
{
    public async Task SignUpAsync(Users user)
    {
        try
        {
            Logs.LogAddingUserUserEmailToDatabase(logger, user.Email);
            context.User.Add(user);
            await context.SaveChangesAsync();
            Logs.LogAddedUserUserEmailToDatabase(logger, user.Email);
        }
        catch (Exception ex)
        {
            Logs.LogCouldNotAddUserUserEmailToDatabaseBecauseExMessage(logger, ex, user.Email, ex.Message);
            throw;
        }
    }

    public async Task<Users?> GetUserByEmailAsync(string email)
    {
        return await context.User.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> ActivateUserAsync(string email, Guid providedKey)
    {
        var user = await context.User.FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
            return false;

        if (user.ActivationKey != providedKey)
            return false;

        user.IsActive = true;
        user.UpdatedAt = DateTime.UtcNow;
        user.ActivationKey = null;

        await context.SaveChangesAsync();
        return true;
    }
}