using GoodStuff.UserApi.Application.Models;
using GoodStuff.UserApi.Application.Services.Interfaces;
using GoodStuff.UserApi.Domain.Entities;
using GoodStuff.UserApi.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace GoodStuff.UserApi.Application.Services;

public class UserService(
    IUserRepository userRepository,
    IEmailNotificationFunctionClient emailNotificationFunctionClient,
    IPasswordService passwordService,
    IGuidProvider guidProvider,
    IUserSessionService userSessionService,
    ILogger<UserService> logger) : IUserService
{
    public async Task<bool> SignUpAsync(User user)
    {
        try
        {
            Logs.LogStartingSignupForEmailEmail(logger, user.Email.Value);

            var existingUser = await userRepository.GetUserByEmailAsync(user.Email);

            if (existingUser != null)
            {
                Logs.LogUserWithEmailEmailAlreadyExists(logger, user.Email.Value);
                return false;
            }

            var activationKey = guidProvider.Get();
            user.SetActivationKey(activationKey);
            
            var passwordHash = passwordService.HashPassword(user.Password.Value);
            user.SetPasswordHash(passwordHash);
            
            await userRepository.SignUpAsync(user);
            Logs.LogNewUserCreatedEmailEmailActivationKey(logger, user.Email.Value, user.ActivationKey!.Value);

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

    public async Task<UserSession?> SignInAsync(Email email, Password password)
    {
        try
        {
            Logs.LogAttemptingSigninForEmailEmail(logger, email.Value);

            var user = await userRepository.GetUserByEmailAsync(email);
            if (user == null) return null;
            
            if (!passwordService.VerifyPassword(password.Value, user.Password.Value))
            {
                if (!user.IsActive)
                {
                    logger.LogUserWithEmailEmailIsNotActive(email.Value);
                    return null;
                }

                Logs.LogInvalidCredentialsForEmail(logger, email.Value);
                return null;
            }
            
            var session = userSessionService.CreateSession(user); 
            Logs.LogUserEmailSuccessfullySignedIn(logger, email.Value);
            
            return session;
        }
        catch (Exception ex)
        {
            Logs.LogErrorOccurredDuringSigninForEmail(logger, ex, email.Value);
            throw;
        }
    }

    public async Task<User?> GetUserByEmailAsync(Email email)
    {
        try
        {
            Logs.LogFetchingUserByEmailEmail(logger, email.Value);
            return await userRepository.GetUserByEmailAsync(email);
        }
        catch (Exception ex)
        {
            Logs.LogErrorFetchingUserByEmailEmail(logger, ex, email.Value);
            throw;
        }
    }

    public async Task<bool> ActivateUserAsync(Email email, ActivationToken providedKey)
    {
        try
        {
            Logs.LogAttemptingToActivateUserEmailWithKeyKey(logger, email.Value, providedKey.Value);

            var result = await userRepository.ActivateUserAsync(email, providedKey);

            if (result)
                Logs.LogUserEmailSuccessfullyActivated(logger, email.Value);
            else
                Logs.LogActivationFailedInvalidKeyForEmail(logger, email.Value);

            return result;
        }
        catch (Exception ex)
        {
            Logs.LogErrorOccurredWhileActivatingUserEmailWithKeyKey(logger, ex, email.Value, providedKey.Value);
            throw;
        }
    }

    public async Task RemoveUserAsync(Email email)
    {
        await userRepository.RemoveUserAsync(email);
    }
}