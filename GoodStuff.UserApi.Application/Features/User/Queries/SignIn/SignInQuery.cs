using GoodStuff.UserApi.Domain.Models.User;
using MediatR;

namespace GoodStuff.UserApi.Application.Features.User.Queries.SignIn;

public record SignInQuery : IRequest<Domain.Entities.User?>
{
    public string Email { get; init; }
    public string Password { get; init; }
}