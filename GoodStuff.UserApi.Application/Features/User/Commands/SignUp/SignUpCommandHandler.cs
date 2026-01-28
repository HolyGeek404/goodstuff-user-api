using GoodStuff.UserApi.Application.Services.Interfaces;
using GoodStuff.UserApi.Domain.ValueObjects;
using MediatR;

namespace GoodStuff.UserApi.Application.Features.User.Commands.SignUp;

public class SignUpCommandHandler(IUserService userService) : IRequestHandler<SignUpCommand, bool>
{
    public Task<bool> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        var name = Name.Create(request.Name);
        var surname = Name.Create(request.Surname);
        var email = Email.Create(request.Email);
        var password = Password.Create(request.Password);
        var user = Domain.Entities.User.Create(name, surname, email, password);
        return userService.SignUpAsync(user);
    }
}