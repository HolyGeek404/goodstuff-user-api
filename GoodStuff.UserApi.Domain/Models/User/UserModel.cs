using GoodStuff.UserApi.Domain.ValueObjects;

namespace GoodStuff.UserApi.Domain.Models.User;

public class Session
{
    public string Id { get; set; }
    public Name Name { get; set; }
    public Name Surname { get; set; }
    public Email Email { get; set; }
}