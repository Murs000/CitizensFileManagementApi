using CitizenFileManagement.Core.Application.Common.Exceptions;
using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Infrastructure.External.Helpers;
using MediatR;

namespace CitizenFileManagement.Core.Application.Features.Commands.User.Password;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, bool>
{
    private readonly IUserRepository _userRepository;

    public ResetPasswordCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(x => x.Email == request.Email);

        if (user == null)
        {
            throw new NotFoundException("Email not found.");
        }

        var (hash, salt) = PasswordHelper.HashPassword(request.NewPassword);
        user.SetPassword(hash, salt);
        user.OTP = null; // Clear OTP after reset
        await _userRepository.SaveAsync();

        return true;
    }
}