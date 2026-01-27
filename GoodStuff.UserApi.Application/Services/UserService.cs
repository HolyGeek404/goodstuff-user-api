using GoodStuff.UserApi.Application.Features.User.Commands.SignUp;
using GoodStuff.UserApi.Application.Services.Interfaces;
using GoodStuff.UserApi.Domain.Models.User;
using Microsoft.Extensions.Logging;

namespace GoodStuff.UserApi.Application.Services;

public class UserService(
    IUserRepository userRepository,
    IEmailNotificationFunctionClient emailNotificationFunctionClient,
    IPasswordService passwordService,
    IGuidProvider guidProvider,
    ILogger<UserService> logger) : IUserService
{
    public async Task<bool> SignUpAsync(SignUpCommand model)
    {
        try
        {
            Logs.LogStartingSignupForEmailEmail(logger, model.Email);

            var existingUser = await userRepository.GetUserByEmailAsync(model.Email);

            if (existingUser != null)
            {
                Logs.LogUserWithEmailEmailAlreadyExists(logger, model.Email);
                return false;
            }

            var activationKey = guidProvider.Get();

            var user = new Users
            {
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                Password = passwordService.HashPassword(model.Password),
                CreatedAt = DateTime.UtcNow,
                IsActive = false,
                ActivationKey = activationKey
            };

            await userRepository.SignUpAsync(user);

            Logs.LogNewUserCreatedEmailEmailActivationKey(logger, user.Email, user.ActivationKey);

            await emailNotificationFunctionClient.SendVerificationEmail(user.Email, activationKey);

            Logs.LogVerificationEmailSentToEmail(logger, user.Email);

            return true;
        }
        catch (Exception ex)
        {
            Logs.LogErrorOccurredDuringSignupForEmail(logger, ex, model.Email);
            throw;
        }
    }

    public async Task<Users?> SignInAsync(string email, string password)
    {
        try
        {
            Logs.LogAttemptingSigninForEmailEmail(logger, email);

            var user = await userRepository.GetUserByEmailAsync(email);

            if (user != null && passwordService.VerifyPassword(password, user.Password))
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

    public async Task<Users?> GetUserByEmailAsync(string email)
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