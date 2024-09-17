using CitizenFileManagement.Core.Application.Features.Commands.User.ViewModels;
using MediatR;

namespace CitizenFileManagement.Core.Application.Features.Commands.User.RefreshToken;

public class RefreshTokenCommand : IRequest<JwtTokenDto>
{
    public string RefreshToken { get; set; }
}