namespace GoodStuff.UserApi.Domain.Entities;

public record User
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public required bool IsActive { get; set; }
    public Guid? ActivationKey { get; set; }
}