using GoodStuff.UserApi.Application.Services;
using GoodStuff.UserApi.Application.Services.Interfaces;
using GoodStuff.UserApi.Domain.Entities;
using GoodStuff.UserApi.Infrastructure.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GoodStuff.UserApi.Infrastructure.DataAccess;

public class UserRepository(GoodStuffContext context, ILogger<UserRepository> logger) : IUserRepository
{
    public async Task SignUpAsync(User user)
    {
        try
        {
            Logs.LogAddingUserUserEmailToDatabase(logger, user.Email.Value);
            context.User.Add(user);
            await context.SaveChangesAsync();
            Logs.LogAddedUserUserEmailToDatabase(logger, user.Email.Value);
        }
        catch (Exception ex)
        {
            Logs.LogCouldNotAddUserUserEmailToDatabaseBecauseExMessage(logger, ex, user.Email.Value, ex.Message);
            throw;
        }
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await context.User.FirstOrDefaultAsync(u => u.Email.Value == email);
    }

    public async Task<bool> ActivateUserAsync(string email, Guid providedKey)
    {
        var user = await context.User.FirstOrDefaultAsync(u => u.Email.Value == email);

        if (user == null)
            return false;

        if (user.Token.Value != providedKey)
            return false;

        user.Activate();
        await context.SaveChangesAsync();
        return true;
    }
}