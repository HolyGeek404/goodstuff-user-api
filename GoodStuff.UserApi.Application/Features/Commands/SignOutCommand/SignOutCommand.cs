using MediatR;

namespace GoodStuff.UserApi.Application.Features.Commands.SignOutCommand;

public class SignOutCommand : IRequest<bool>
{
    public string SessionId { get; set; }
}