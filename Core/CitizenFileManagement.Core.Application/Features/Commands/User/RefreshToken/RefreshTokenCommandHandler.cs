using CitizenFileManagement.Core.Application.Exceptions;
using CitizenFileManagement.Core.Application.Features.Commands.User.ViewModels;
using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Infrastructure.External.Helpers;
using MediatR;

namespace CitizenFileManagement.Core.Application.Features.Commands.User.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, JwtTokenDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserManager _userManager;

    public RefreshTokenCommandHandler(IUserRepository userRepository, IUserManager userManager)
    {
        _userRepository = userRepository;
        _userManager = userManager;
    }

    public async Task<JwtTokenDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(x => x.RefreshToken == request.RefreshToken);

        if (user == null || user.RefreshTokenExpireDate <= DateTime.UtcNow || user.IsDeleted)
        {
            throw new UnAuthorizedException("Invalid or expired refresh token.");
        }

        var newRefreshToken = RefreshTokenHelper.GenerateRefreshToken(user.Id);
        user.SetRefreshToken(newRefreshToken, DateTime.UtcNow.AddHours(4).AddDays(20));
        await _userRepository.SaveAsync();

        var (token, expireAt) = _userManager.GenerateTJwtToken(user);

        return new JwtTokenDto
        {
            Token = token,
            RefreshToken = newRefreshToken,
            ExpireAt = expireAt
        };
    }
}