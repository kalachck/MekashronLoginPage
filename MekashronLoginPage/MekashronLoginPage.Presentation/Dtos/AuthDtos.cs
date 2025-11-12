using MekashronLoginPage.Application;

namespace MekashronLoginPage.Presentation.Dtos;

public class LoginResponseDto
{
    public bool IsAuthorized { get; set; }

    public AuthorizationProvider.LoginResponseData? LoginResponseData { get; set; }
}
