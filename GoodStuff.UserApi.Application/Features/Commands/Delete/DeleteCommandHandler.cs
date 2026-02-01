using GoodStuff.UserApi.Application.Services.Interfaces;
using GoodStuff.UserApi.Domain.ValueObjects;
using MediatR;

namespace GoodStuff.UserApi.Application.Features.Commands.Delete;

public class DeleteCommandHandler(IUserService userService) : IRequestHandler<DeleteCommand>
{
    public Task Handle(DeleteCommand request, CancellationToken cancellationToken)
    {
        var email = Email.Create(request.Email);
        userService.RemoveUserAsync(email);
        return Task.CompletedTask;
    }
}