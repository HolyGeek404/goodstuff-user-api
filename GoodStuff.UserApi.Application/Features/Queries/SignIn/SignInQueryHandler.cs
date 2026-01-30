using GoodStuff.UserApi.Application.Models;
using GoodStuff.UserApi.Application.Services.Interfaces;
using GoodStuff.UserApi.Domain.ValueObjects;
using MediatR;

namespace GoodStuff.UserApi.Application.Features.Queries.SignIn;

public class SignInQueryHandler(IUserService userService) : IRequestHandler<SignInQuery, UserSession?>
{
    public Task<UserSession?> Handle(SignInQuery request, CancellationToken cancellationToken)
    {
        var email = Email.Create(request.Email);
        var password = Password.Create(request.Password);
        
        return userService.SignInAsync(email, password);
    }
}