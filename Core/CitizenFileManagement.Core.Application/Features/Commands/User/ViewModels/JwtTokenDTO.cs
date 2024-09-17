namespace CitizenFileManagement.Core.Application.Features.Commands.User.ViewModels;

public class JwtTokenDto
{
    public string Token { get; set; }
    public string RefreshToken{ get; set; }
    public DateTime ExpireAt { get; set; }
}
