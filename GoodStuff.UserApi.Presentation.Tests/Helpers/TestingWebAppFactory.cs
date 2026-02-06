using GoodStuff.UserApi.Application.Services.Interfaces;
using GoodStuff.UserApi.Infrastructure.DataAccess.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace GoodStuff.UserApi.Presentation.Tests.Helpers;

public class TestingWebAppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var guid = new Guid("a0b25888-3f42-489a-80b5-f262effabd25");

            var guidProviderMock = new Mock<IGuidProvider>();
            guidProviderMock.Setup(x => x.Get()).Returns(guid);

            services.AddSingleton(guidProviderMock.Object);
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
        });
    }
}