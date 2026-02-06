using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MediatR;

namespace GoodStuff.UserApi.Application.Features.Commands.AccountVerification;

public class AccountVerificationCommand : IRequest<bool>
{
    [JsonPropertyName("userEmail")]
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [JsonPropertyName("key")] [Required] public required Guid VerificationKey { get; set; }
}