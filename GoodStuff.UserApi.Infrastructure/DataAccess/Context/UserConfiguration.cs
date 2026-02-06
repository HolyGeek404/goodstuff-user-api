using GoodStuff.UserApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodStuff.UserApi.Infrastructure.DataAccess.Context;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.Name, n =>
        {
            n.Property(p => p.Value)
                .HasColumnName("Name")
                .HasMaxLength(100)
                .IsRequired();
        });

        builder.OwnsOne(x => x.Surname, n =>
        {
            n.Property(p => p.Value)
                .HasColumnName("Surname")
                .HasMaxLength(100)
                .IsRequired();
        });

        builder.OwnsOne(x => x.Email, e =>
        {
            e.Property(p => p.Value)
                .HasColumnName("Email")
                .HasMaxLength(256)
                .IsRequired();

            e.HasIndex(p => p.Value)
                .IsUnique();
        });

        builder.OwnsOne(x => x.Password, p =>
        {
            p.Property(x => x.Value)
                .HasColumnName("PasswordHash")
                .IsRequired();
        });

        builder.OwnsOne(x => x.ActivationKey, k =>
        {
            k.Property(p => p.Value)
                .HasColumnName("ActivationKey");
        });

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt);

        builder.Property(x => x.IsActive)
            .IsRequired();
    }
}