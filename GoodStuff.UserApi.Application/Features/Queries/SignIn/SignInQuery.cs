using System.ComponentModel.DataAnnotations;
using MediatR;

namespace GoodStuff.UserApi.Application.Features.Queries.SignIn;

public record SignInQuery : IRequest<string?>
{
    [Required] [EmailAddress] public required string Email { get; init; }
    [Required] public required string Password { get; init; }
}
