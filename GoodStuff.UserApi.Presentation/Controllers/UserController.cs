using GoodStuff.UserApi.Application.Features.User.Commands.AccountVerification;
using GoodStuff.UserApi.Application.Features.User.Commands.SignOutCommand;
using GoodStuff.UserApi.Application.Features.User.Commands.SignUp;
using GoodStuff.UserApi.Application.Features.User.Queries.SignIn;
using GoodStuff.UserApi.Application.Services;
using GoodStuff.UserApi.Application.Services.Interfaces;
using GoodStuff.UserApi.Domain.Models.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoodStuff.UserApi.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IMediator mediator, ILogger<UserController> logger, IUserSessionService sessionService)
    : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "SignUp")]
    [Route("signup")]
    public async Task<IActionResult> SignUp([FromBody] SignUpCommand signUpCommand)
    {
        Logs.LogCalledSignupNameByUnknown(logger, nameof(SignUp), User.FindFirst("appid")?.Value ?? "Unknown");
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var result = await mediator.Send(signUpCommand);
            if (result)
            {
                Logs.LogSuccessfullyRegisteredNewUserEmailCalledByUnknown(logger, signUpCommand.Email, User.FindFirst("appid")?.Value ?? "Unknown");
                var userModel = new UserModel
                {
                    Email = signUpCommand.Email,
                    Name = signUpCommand.Name,
                    Surname = signUpCommand.Surname
                };
                return CreatedAtAction(nameof(SignIn), new { email = signUpCommand.Email }, userModel);
            }
        }
        catch (Exception e)
        {
            Logs.LogAnErrorOccurredWhileSigningUp(logger, e);
            return StatusCode(500, "An error occurred while signing up.");
        }

        Logs.LogCouldNotRegisterUserEmailCalledByUnknown(logger, signUpCommand.Email, User.FindFirst("appid")?.Value ?? "Unknown");
        return BadRequest();
    }

    [HttpPost]
    [Authorize(Roles = "SignIn")]
    [Route("signin")]
    public async Task<IActionResult> SignIn([FromBody] SignInQuery signInQuery)
    {
        Logs.LogCalledSignupNameByUnknown(logger, nameof(SignUp), User.FindFirst("appid")?.Value ?? "Unknown");

        if (string.IsNullOrEmpty(signInQuery.Email) || string.IsNullOrEmpty(signInQuery.Password))
        {
            Logs.LogCouldNotSignInBecauseEmailOrPasswordIsEmpty(logger);
            return BadRequest("Email or password is empty.");
        }

        var user = await mediator.Send(signInQuery);

        if (user == null) return Unauthorized();

        var sessionId = sessionService.CreateSession(user);
        var userModel = new UserModel
        {
            Email = user.Email,
            Name = user.Name,
            Surname = user.Surname,
            SessionId = sessionId
        };

        Logs.LogSuccessfullySignedInUserEmailCalledSignupNameByUnknown(logger, signInQuery.Email, nameof(SignUp),
            User.FindFirst("appid")?.Value ?? "Unknown");
        return Ok(userModel);
    }

    [HttpPost]
    [Route("signout")]
    [Authorize(Roles = "SignOut")]
    public async Task<IActionResult> SignOut([FromBody] SignOutCommand signOutCommand)
    {
        Logs.LogSignoutRequestReceivedSessionId(logger, signOutCommand);

        try
        {
            Logs.LogClearingCachedUserDataForSessionId(logger, signOutCommand);

            var result = await mediator.Send(signOutCommand);

            Logs.LogSuccessfullySignedOutUserSessionId(logger, signOutCommand);

            return Ok(true);
        }
        catch (Exception ex)
        {
            Logs.LogAnErrorOccurredWhileSigningOutUserSessionId(logger, ex, signOutCommand.SessionId);
            return StatusCode(500, "Internal server error during sign-out.");
        }
    }

    [HttpPost]
    [Route("accountverification")]
    public async Task<IActionResult> AccountVerification([FromBody] AccountVerificationCommand accountVerificationCommand)
    {
        try
        {
            var result = await mediator.Send(accountVerificationCommand);

            Logs.LogAccountVerificationSuccessfulForAccountAccount(logger, accountVerificationCommand.Email);

            return Ok(result);
        }
        catch (Exception ex)
        {
            Logs.LogErrorOccurredDuringVerificationForAccountAccount(logger, ex, accountVerificationCommand.Email);
            return StatusCode(500, "An error occurred while verifying the account.");
        }
    }
}