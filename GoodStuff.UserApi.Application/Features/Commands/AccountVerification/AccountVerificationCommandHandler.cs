using GoodStuff.UserApi.Application.Services.Interfaces;
using GoodStuff.UserApi.Domain.ValueObjects;
using MediatR;

namespace GoodStuff.UserApi.Application.Features.Commands.AccountVerification;

public class AccountVerificationCommandHandler(IUserService userService)
    : IRequestHandler<AccountVerificationCommand, bool>
{
    public async Task<bool> Handle(AccountVerificationCommand request, CancellationToken cancellationToken)
    {
        var email = Email.Create(request.Email);
        var token = ActivationToken.Create(request.VerificationKey);
        
        var result = await userService.ActivateUserAsync(email, token);
        return result;
    }
}