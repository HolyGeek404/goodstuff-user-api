using GoodStuff.UserApi.Domain.Entities;
using GoodStuff.UserApi.Domain.Models.User;

namespace GoodStuff.UserApi.Application.Services.Interfaces;

public interface IUserSessionService
{
    string CreateSession(User user);
    UserSession? GetUserSession();
    bool Validate();
    void ClearUserCachedData(string sessionId);
}