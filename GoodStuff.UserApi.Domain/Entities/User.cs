using GoodStuff.UserApi.Domain.ValueObjects;

namespace GoodStuff.UserApi.Domain.Entities;

public class User
{
    public int Id { get; private set; }
    public Name Name { get; private set; }
    public Name Surname { get; private set; }
    public Email Email { get; private set; }
    public Password Password { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    
    public bool IsActive { get; private set; }
    public ActivationToken? ActivationKey { get; private set; }

    private User() { }

    public static User Create(Name name, Name surname, Email email, Password password)
    {
        return new User
        {
            Name = name,
            Surname = surname,
            Email = email,
            Password = password,
            CreatedAt = DateTime.UtcNow,
            IsActive = false
        };
    }

    public void SetActivationKey(Guid activationKey)
    {
        if(IsActive)
            throw new InvalidOperationException("Cannot set activation key while User is already active.");
        ActivationKey = ActivationToken.Create(activationKey);
    }

    public void SetPasswordHash(string passwordHash)
    {
        if (IsActive)
            throw new InvalidOperationException("Cannot set password while User is already active.");
        Password = new Password(passwordHash);
    }

    public void Activate()
    {
        IsActive = true;
        Updated();
        ActivationKey = null;
    }
    
    private void Updated()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}