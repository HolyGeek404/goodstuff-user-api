using GoodStuff.UserApi.Presentation.Extensions;

namespace GoodStuff.UserApi.Presentation;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddServices();
        builder.Services.AddMediatrConfig();
        builder.Services.AddAzureConfig(builder.Configuration);
        builder.Services.AddDataBaseConfig(builder.Configuration);
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerConfig(builder.Configuration);
        builder.Services.AddMemoryCache();
        builder.Logging.AddLoggingConfig();
        builder.Services.AddHttpClient();
        var app = builder.Build();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "GoodStuff User Api v1");
                c.OAuthClientId(builder.Configuration["Swagger:SwaggerClientId"]);
                c.OAuthUsePkce();
                c.OAuthScopeSeparator(" ");
            }
        );
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}