using System.Security.Cryptography;
using CitizenFileManagement.Core.Application.Exceptions;
using CitizenFileManagement.Core.Application.Features.Commands.User.ViewModels;
using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Infrastructure.External.Helpers;
using MediatR;

namespace CitizenFileManagement.Core.Application.Features.Commands.User.Login;

public class CreateAuthorizationTokenCommandHandler : IRequestHandler<CreateAuthorizationTokenCommand, JwtTokenDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserManager _userManager;
    public CreateAuthorizationTokenCommandHandler(IUserRepository userRepository, IUserManager userManager)
    {
        _userRepository = userRepository;
        _userManager = userManager;
    }

    public async Task<JwtTokenDto> Handle(CreateAuthorizationTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(x => x.Username.ToLower() == request.Username.ToLower());

        if (user == null
            || !PasswordHelper.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt)
            || !user.IsActivated
            || user.IsDeleted)
        {
            throw new UnAuthorizedException("Invalid credentials");
        }

        var random = GenerateRandomNumber();
        var refreshToken = $"{random}_{user.Id}_{DateTime.UtcNow.AddDays(20)}";
        user.SetRefreshToken(refreshToken,DateTime.UtcNow.AddHours(4).AddDays(20));
        (string token, DateTime expireAt) = _userManager.GenerateTJwtToken(user);
        await _userRepository.SaveAsync();

        return new JwtTokenDto
        {
            ExpireAt = expireAt,
            RefreshToken = refreshToken,
            Token = token
        };
    }

    private object GenerateRandomNumber()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}