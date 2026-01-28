using GoodStuff.UserApi.Application.Services.Interfaces;
using GoodStuff.UserApi.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace GoodStuff.UserApi.Application.Services;

public class UserService(
    IUserRepository userRepository,
    IEmailNotificationFunctionClient emailNotificationFunctionClient,
    IPasswordService passwordService,
    IGuidProvider guidProvider,
    ILogger<UserService> logger) : IUserService
{
    public async Task<bool> SignUpAsync(User user)
    {
        try
        {
            Logs.LogStartingSignupForEmailEmail(logger, user.Email.Value);

            var existingUser = await userRepository.GetUserByEmailAsync(user.Email.Value);

            if (existingUser != null)
            {
                Logs.LogUserWithEmailEmailAlreadyExists(logger, user.Email.Value);
                return false;
            }

            var activationKey = guidProvider.Get();
            user.SetActivationKey(activationKey);

            await userRepository.SignUpAsync(user);
            Logs.LogNewUserCreatedEmailEmailActivationKey(logger, user.Email.Value, user.Token!.Value);

            await emailNotificationFunctionClient.SendVerificationEmail(user.Email.Value, activationKey);
            Logs.LogVerificationEmailSentToEmail(logger, user.Email.Value);

            return true;
        }
        catch (Exception ex)
        {
            Logs.LogErrorOccurredDuringSignupForEmail(logger, ex, user.Email.Value);
            throw;
        }
    }

    public async Task<User?> SignInAsync(string email, string password)
    {
        try
        {
            Logs.LogAttemptingSigninForEmailEmail(logger, email);

            var user = await userRepository.GetUserByEmailAsync(email);

            if (user != null && passwordService.VerifyPassword(password, user.Password.Value))
            {
                Logs.LogUserEmailSuccessfullySignedIn(logger, email);
                return user;
            }

            if (user is { IsActive: false })
            {
                logger.LogUserWithEmailEmailIsNotActive(email);
                return null;
            }

            Logs.LogInvalidCredentialsForEmail(logger, email);
            return null;
        }
        catch (Exception ex)
        {
            Logs.LogErrorOccurredDuringSigninForEmail(logger, ex, email);
            throw;
        }
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        try
        {
            Logs.LogFetchingUserByEmailEmail(logger, email);
            return await userRepository.GetUserByEmailAsync(email);
        }
        catch (Exception ex)
        {
            Logs.LogErrorFetchingUserByEmailEmail(logger, ex, email);
            throw;
        }
    }

    public async Task<bool> ActivateUserAsync(string email, Guid providedKey)
    {
        try
        {
            Logs.LogAttemptingToActivateUserEmailWithKeyKey(logger, email, providedKey);

            var result = await userRepository.ActivateUserAsync(email, providedKey);

            if (result)
                Logs.LogUserEmailSuccessfullyActivated(logger, email);
            else
                Logs.LogActivationFailedInvalidKeyForEmail(logger, email);

            return result;
        }
        catch (Exception ex)
        {
            Logs.LogErrorOccurredWhileActivatingUserEmailWithKeyKey(logger, ex, email, providedKey);
            throw;
        }
    }
}