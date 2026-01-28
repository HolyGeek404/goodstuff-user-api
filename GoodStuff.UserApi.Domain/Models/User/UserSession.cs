namespace GoodStuff.UserApi.Domain.Models.User;

public class UserSession
{
    public Entities.User UserData { get; init; } = null!;
    public List<string> Roles { get; set; } = [];
    public DateTime LoginTime { get; set; }
    public DateTime LastActivity { get; set; }
    public string IpAddress { get; init; } = string.Empty;
}