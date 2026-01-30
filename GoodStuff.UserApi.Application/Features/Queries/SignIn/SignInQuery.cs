using System.ComponentModel.DataAnnotations;
using GoodStuff.UserApi.Application.Models;
using MediatR;

namespace GoodStuff.UserApi.Application.Features.Queries.SignIn;

public record SignInQuery : IRequest<UserSession>
{
    [Required] [EmailAddress] public required string Email { get; init; }
    [Required] public required string Password { get; init; }
}