using GoodStuff.UserApi.Application.Features.Commands.AccountVerification;
using GoodStuff.UserApi.Application.Features.Commands.Delete;
using GoodStuff.UserApi.Application.Features.Commands.SignOutCommand;
using GoodStuff.UserApi.Application.Features.Commands.SignUp;
using GoodStuff.UserApi.Application.Features.Queries.SignIn;
using GoodStuff.UserApi.Application.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoodStuff.UserApi.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IMediator mediator, ILogger<UserController> logger) : ControllerBase
{
    [HttpPost]
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
                Logs.LogSuccessfullyRegisteredNewUserEmailCalledByUnknown(logger, signUpCommand.Email,
                    User.FindFirst("appid")?.Value ?? "Unknown");
                return CreatedAtAction(nameof(SignIn), new { email = signUpCommand.Email });
            }
        }
        catch (Exception e)
        {
            logger.LogAnErrorOccurredWhileSigningUp(e);
            return StatusCode(500, "An error occurred while signing up.");
        }

        Logs.LogCouldNotRegisterUserEmailCalledByUnknown(logger, signUpCommand.Email,
            User.FindFirst("appid")?.Value ?? "Unknown");
        return BadRequest();
    }

    [HttpPost]
    [Route("signin")]
    public async Task<IActionResult> SignIn([FromBody] SignInQuery signInQuery)
    {
        Logs.LogCalledSignupNameByUnknown(logger, nameof(SignUp), User.FindFirst("appid")?.Value ?? "Unknown");

        if (!ModelState.IsValid)
        {
            Logs.LogCouldNotSignInBecauseEmailOrPasswordIsEmpty(logger);
            return BadRequest("Email or password is empty.");
        }

        var token = await mediator.Send(signInQuery);
        if (string.IsNullOrWhiteSpace(token))
        {
            return Unauthorized();
        }

        Response.Cookies.Append("access_token", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Path = "/",
            MaxAge = TimeSpan.FromHours(1)
        });

        Logs.LogSuccessfullySignedInUserEmailCalledSignupNameByUnknown(logger, signInQuery.Email, nameof(SignUp),
            User.FindFirst("appid")?.Value ?? "Unknown");
        return Ok();
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

            return Ok(result);
        }
        catch (Exception ex)
        {
            Logs.LogAnErrorOccurredWhileSigningOutUserSessionId(logger, ex, signOutCommand.SessionId);
            return StatusCode(500, "Internal server error during sign-out.");
        }
    }

    [HttpPost]
    [Route("accountverification")]
    public async Task<IActionResult> AccountVerification(
        [FromBody] AccountVerificationCommand accountVerificationCommand)
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

    [HttpDelete]
    [Route("delete")]
    public async Task<IActionResult> Delete([FromQuery] string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return BadRequest("Email cannot be null or empty.");

        try
        {
            await mediator.Send(new DeleteCommand { Email = email });
            return NoContent();
        }
        catch (Exception e)
        {
            return StatusCode(500, "An error occurred while deleting the account.");
        }
    }
}
