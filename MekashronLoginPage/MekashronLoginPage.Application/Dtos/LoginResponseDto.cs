using System.Text.Json.Serialization;
using MekashronLoginPage.Application.Utils;

namespace MekashronLoginPage.Application.Dtos;

public class LoginResponseDto
{
    public int EntityId { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Company { get; init; }
    public string? Address { get; init; }
    public string? City { get; init; }
    public string? Country { get; init; }
    public string? Zip { get; init; }
    public string? Phone { get; init; }
    public string? Mobile { get; init; }
    public string? Email { get; init; }
    public int EmailConfirm { get; init; }
    public int MobileConfirm { get; init; }
    public int CountryID { get; init; }
    public int Status { get; init; }

    [JsonPropertyName(Constants.JsonPropertyLid)]
    public string? Lid { get; init; }

    public string? FTPHost { get; init; }
    public int FTPPort { get; init; }
}

