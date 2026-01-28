using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using GoodStuff.UserApi.Application.Features.User.Commands.AccountVerification;
using GoodStuff.UserApi.Application.Features.User.Commands.SignUp;
using GoodStuff.UserApi.Application.Features.User.Queries.SignIn;
using GoodStuff.UserApi.Application.Services;
using GoodStuff.UserApi.Domain.Models.User;
using GoodStuff.UserApi.Presentation.Tests.Helpers;
using Moq;

namespace GoodStuff.UserApi.Presentation.Tests.Controllers;

public class UserControllerTests(TestingWebAppFactory factory) : IClassFixture<TestingWebAppFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    private readonly SignUpCommand _signUpCommand = new()
    {
        Email = "test@example.com",
        Name = "John",
        Surname = "Doe",
        Password = "Password1@34"
    };

    [Fact]
    public async Task SignIn_Should_Succeed_And_Return_SessionId()
    {
        // Arrange
        var guidProvider = new Mock<IGuidProvider>();
        var guid = new Guid("a0b25888-3f42-489a-80b5-f262effabd25");
        guidProvider.Setup(x => x.Get()).Returns(guid);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        await _client.PostAsJsonAsync("/User/signup", _signUpCommand);

        var authCommand = new AccountVerificationCommand
        {
            Email = _signUpCommand.Email,
            VerificationKey = guid
        };
        await _client.PostAsJsonAsync("/User/accountverification", authCommand);

        var query = new SignInQuery
        {
            Email = _signUpCommand.Email,
            Password = _signUpCommand.Password
        };

        // Act
        var response = await _client.PostAsJsonAsync("/User/signin", query);
        response.EnsureSuccessStatusCode();

        // Assert
        var model = await response.Content.ReadFromJsonAsync<Session>();
        Assert.Equal(_signUpCommand.Email, model?.Email.Value);
    }

    [Fact]
    public async Task SignIn_Should_Return_BadRequest_When_Email_Empty()
    {
        // Arrange
        var badQuery = new SignInQuery
        {
            Email = "",
            Password = _signUpCommand.Password
        };
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        // Act
        var response = await _client.PostAsJsonAsync("/User/signin", badQuery);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("Email or password is empty.", content);
    }

    [Fact]
    public async Task SignUp_Should_Return_Created_When_Successful()
    {
        // Arrange
        var signUpCommand = _signUpCommand with { Email = "something@test.com" };
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        // Act
        var response = await _client.PostAsJsonAsync("/User/signup", signUpCommand);
        response.EnsureSuccessStatusCode();

        // Assert
        var content = await response.Content.ReadFromJsonAsync<Session>();
        Assert.Equal(signUpCommand.Email, content?.Email.Value);
    }
}