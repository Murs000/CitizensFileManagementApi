using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Infrastructure.External.Helpers;
using MediatR;

namespace CitizenFileManagement.Core.Application.Features.Commands.User.Register;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;

    public RegisterUserCommandHandler(IUserRepository userRepository, IEmailService emailService)
    {
        _userRepository = userRepository;
        _emailService = emailService;
    }

    public async Task<bool> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = new Domain.Entities.User
        {
            Username = request.Username,
            Email = request.Email,
            IsActivated = false,
        };

        (string hash, string salt) = PasswordHelper.HashPassword(request.Password);
        user.SetPassword(hash, salt);

        var otp = OTPHelper.GenerateOTP();
        user.OTP = otp;

        await _userRepository.AddAsync(user);
        await _userRepository.SaveAsync();

        await _emailService.SendEmailAsync(user.Email, "OTP message", otp);

        return true;
    }
}