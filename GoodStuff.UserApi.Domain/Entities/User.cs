using GoodStuff.UserApi.Domain.ValueObjects;

namespace GoodStuff.UserApi.Domain.Entities;

public class User
{
    public required int Id { get; set; }
    public required Name Name { get; set; }
    public required Name Surname { get; set; }
    public required Email Email { get; set; }
    public required Password Password { get; set; }
    public required DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public required bool IsActive { get; set; }
    public Guid? ActivationKey { get; set; }
}