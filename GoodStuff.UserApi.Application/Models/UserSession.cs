namespace GoodStuff.UserApi.Application.Models;

public sealed class UserSession
{
    public string SessionId { get; init; } = null!;
    public int UserId { get; init; }
    public string Email { get; init; } = null!;
    public DateTime LoginTime { get; init; }
    public DateTime LastActivity { get; set; }
    public string IpAddress { get; init; } = null!;
}
