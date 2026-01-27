using GoodStuff.UserApi.Application.Features.User.Commands.SignOutCommand;
using Microsoft.Extensions.Logging;

namespace GoodStuff.UserApi.Application.Services;

public static partial class Logs
{
    [LoggerMessage(LogLevel.Information, "Starting SignUp for email: {email}")]
    public static partial void LogStartingSignupForEmailEmail(ILogger<UserService> logger, string email);

    [LoggerMessage(LogLevel.Warning, "User with email {email} already exists.")]
    public static partial void LogUserWithEmailEmailAlreadyExists(ILogger<UserService> logger, string email);

    [LoggerMessage(LogLevel.Information, "New user created. Email: {email}, ActivationKey: {activationKey}")]
    public static partial void LogNewUserCreatedEmailEmailActivationKey(ILogger<UserService> logger, string email,
        Guid? activationKey);

    [LoggerMessage(LogLevel.Information, "Verification email sent to {email}")]
    public static partial void LogVerificationEmailSentToEmail(ILogger<UserService> logger, string email);

    [LoggerMessage(LogLevel.Error, "Error occurred during SignUp for {email}")]
    public static partial void LogErrorOccurredDuringSignupForEmail(ILogger<UserService> logger, Exception e,
        string email);

    [LoggerMessage(LogLevel.Information, "Attempting SignIn for email: {email}")]
    public static partial void LogAttemptingSigninForEmailEmail(ILogger<UserService> logger, string email);

    [LoggerMessage(LogLevel.Information, "User {email} successfully signed in.")]
    public static partial void LogUserEmailSuccessfullySignedIn(ILogger<UserService> logger, string email);

    [LoggerMessage(LogLevel.Warning, "Invalid credentials for {email}.")]
    public static partial void LogInvalidCredentialsForEmail(ILogger<UserService> logger, string email);

    [LoggerMessage(LogLevel.Error, "Error occurred during SignIn for {email}")]
    public static partial void LogErrorOccurredDuringSigninForEmail(ILogger<UserService> logger, Exception e,
        string email);

    [LoggerMessage(LogLevel.Information, "Fetching user by email: {email}")]
    public static partial void LogFetchingUserByEmailEmail(ILogger<UserService> logger, string email);

    [LoggerMessage(LogLevel.Error, "Error fetching user by email: {email}")]
    public static partial void LogErrorFetchingUserByEmailEmail(ILogger<UserService> logger, Exception e, string email);

    [LoggerMessage(LogLevel.Information, "Attempting to activate user {email} with key {key}")]
    public static partial void LogAttemptingToActivateUserEmailWithKeyKey(ILogger<UserService> logger, string email,
        Guid key);

    [LoggerMessage(LogLevel.Information, "User {email} successfully activated.")]
    public static partial void LogUserEmailSuccessfullyActivated(ILogger<UserService> logger, string email);

    [LoggerMessage(LogLevel.Warning, "Activation failed. Invalid key for {email}.")]
    public static partial void LogActivationFailedInvalidKeyForEmail(ILogger<UserService> logger, string email);

    [LoggerMessage(LogLevel.Error, "Error occurred while activating user {email} with key {key}")]
    public static partial void LogErrorOccurredWhileActivatingUserEmailWithKeyKey(ILogger<UserService> logger,
        Exception e, string email, Guid key);

    [LoggerMessage(LogLevel.Information, "Called {signUpName} by {unknown}")]
    public static partial void LogCalledSignupNameByUnknown(ILogger logger, string signUpName, string unknown);

    [LoggerMessage(LogLevel.Information, "Successfully registered new user {email}. Called by {unknown}")]
    public static partial void LogSuccessfullyRegisteredNewUserEmailCalledByUnknown(ILogger logger, string email,
        string unknown);

    [LoggerMessage(LogLevel.Information, "Couldn't register user {email}. Called by {unknown}")]
    public static partial void
        LogCouldNotRegisterUserEmailCalledByUnknown(ILogger logger, string email, string unknown);

    [LoggerMessage(LogLevel.Information, "Couldn't sign in because email or password is empty")]
    public static partial void LogCouldNotSignInBecauseEmailOrPasswordIsEmpty(ILogger logger);

    [LoggerMessage(LogLevel.Information, "Successfully signed in user {email}. Called {signUpName} by {unknown}")]
    public static partial void LogSuccessfullySignedInUserEmailCalledSignupNameByUnknown(ILogger logger, string email,
        string signUpName, string unknown);

    [LoggerMessage(LogLevel.Information, "SignOut request received. SessionId: {sessionId}")]
    public static partial void LogSignoutRequestReceivedSessionId(ILogger logger, SignOutCommand sessionId);

    [LoggerMessage(LogLevel.Information, "Clearing cached user data for SessionId: {sessionId}")]
    public static partial void LogClearingCachedUserDataForSessionId(ILogger logger, SignOutCommand sessionId);

    [LoggerMessage(LogLevel.Information, "Successfully signed out user. SessionId: {sessionId}")]
    public static partial void LogSuccessfullySignedOutUserSessionId(ILogger logger, SignOutCommand sessionId);

    [LoggerMessage(LogLevel.Error, "An error occurred while signing out user. SessionId: {SessionId}")]
    public static partial void LogAnErrorOccurredWhileSigningOutUserSessionId(ILogger logger,
        Exception ex, string sessionId);

    [LoggerMessage(LogLevel.Information, "Account verification successful for account: {account}")]
    public static partial void LogAccountVerificationSuccessfulForAccountAccount(ILogger logger, string account);

    [LoggerMessage(LogLevel.Error, "Error occurred during verification for account: {Account}")]
    public static partial void LogErrorOccurredDuringVerificationForAccountAccount(ILogger logger, Exception ex,
        string account);

    [LoggerMessage(LogLevel.Information, "Adding user {userEmail} to database.")]
    public static partial void LogAddingUserUserEmailToDatabase(ILogger logger, string userEmail);

    [LoggerMessage(LogLevel.Information, "Added user {userEmail} to database.")]
    public static partial void LogAddedUserUserEmailToDatabase(ILogger logger, string userEmail);

    [LoggerMessage(LogLevel.Error, "Couldn't add user {UserEmail} to database because: {exMessage}.")]
    public static partial void LogCouldNotAddUserUserEmailToDatabaseBecauseExMessage(ILogger logger,
        Exception ex, string userEmail, string exMessage);

    [LoggerMessage(LogLevel.Warning, "User with email {email} is not active")]
    public static partial void LogUserWithEmailEmailIsNotActive(this ILogger<UserService> logger, string email);

    [LoggerMessage(LogLevel.Error, "An error occurred while signing up.")]
    public static partial void LogAnErrorOccurredWhileSigningUp(this ILogger logger, Exception ex);
}