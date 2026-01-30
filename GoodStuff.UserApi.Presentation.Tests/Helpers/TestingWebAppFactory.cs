using GoodStuff.UserApi.Infrastructure.DataAccess.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GoodStuff.UserApi.Presentation.Tests.Helpers;

public class TestingWebAppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", "Test");

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType ==
                                                           typeof(DbContextOptions<GoodStuffContext>));

            if (descriptor != null)
                services.Remove(descriptor);

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "Test";
                    options.DefaultChallengeScheme = "Test";
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });


            builder.Configure(app =>
            {
                app.UseAuthentication();
                app.UseAuthorization();
            });
            services.AddDbContext<GoodStuffContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryEmployeeTest");
            });

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            using var appContext = scope.ServiceProvider.GetRequiredService<GoodStuffContext>();
            appContext.Database.EnsureCreated();
        });
    }
}