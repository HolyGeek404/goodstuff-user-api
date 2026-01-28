using GoodStuff.UserApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GoodStuff.UserApi.Infrastructure.DataAccess.Context;

public class GoodStuffContext(DbContextOptions<GoodStuffContext> options) : DbContext(options)
{
    public DbSet<User> User { get; set; }
}