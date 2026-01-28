using GoodStuff.UserApi.Application.Services.Interfaces;
using GoodStuff.UserApi.Domain.Models.User;
using MediatR;

namespace GoodStuff.UserApi.Application.Features.User.Queries.SignIn;

public class SignInQueryHandler(IUserService userService) : IRequestHandler<SignInQuery, Domain.Entities.User?>
{
    public Task<Domain.Entities.User?> Handle(SignInQuery request, CancellationToken cancellationToken)
    {
        return userService.SignInAsync(request.Email, request.Password);
    }
}