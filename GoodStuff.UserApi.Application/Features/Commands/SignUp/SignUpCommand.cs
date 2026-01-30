using System.ComponentModel.DataAnnotations;
using MediatR;

namespace GoodStuff.UserApi.Application.Features.Commands.SignUp;

public record SignUpCommand : IRequest<bool>
{
   [Required] public required string Name { get; init; }
   [Required] public required string Surname { get; init; }
   [Required] public required string Password { get; init; }
   [Required] public required string Email { get; init; }
}