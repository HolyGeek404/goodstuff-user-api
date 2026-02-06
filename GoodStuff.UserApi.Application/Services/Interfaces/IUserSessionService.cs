using GoodStuff.UserApi.Application.Models;
using GoodStuff.UserApi.Domain.Entities;

namespace GoodStuff.UserApi.Application.Services.Interfaces;

public interface IUserSessionService
{
    UserSession CreateSession(User user);
    UserSession? GetUserSession(string? session = null);
    bool Validate();
    void ClearUserCachedData(string sessionId);
}