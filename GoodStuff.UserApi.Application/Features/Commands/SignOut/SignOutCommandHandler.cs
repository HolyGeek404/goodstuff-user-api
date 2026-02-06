using GoodStuff.UserApi.Application.Services.Interfaces;
using MediatR;

namespace GoodStuff.UserApi.Application.Features.Commands.SignOutCommand;

public class SignOutCommandHandler(IUserSessionService sessionService) : IRequestHandler<SignOutCommand, bool>
{
    public Task<bool> Handle(SignOutCommand request, CancellationToken cancellationToken)
    {
        sessionService.ClearUserCachedData(request.SessionId);
        return Task.FromResult(true);
    }
}