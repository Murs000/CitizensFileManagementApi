using CitizenFileManagement.Core.Application.Exceptions;
using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Infrastructure.External.Helpers;
using MediatR;

namespace CitizenFileManagement.Core.Application.Features.Commands.User.Password;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand,bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;

    public ForgotPasswordCommandHandler(IUserRepository userRepository, IEmailService emailService)
    {
        _userRepository = userRepository;
        _emailService = emailService;
    }

    public async Task<bool> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(x => x.Email == request.Email);

        if (user == null)
        {
            throw new NotFoundException("User not found.");
        }

        var otp = OTPHelper.GenerateOTP();
        user.OTP = otp;
        await _userRepository.SaveAsync();

        // Send reset password email
        await _emailService.SendEmailAsync(user.Email, "OTP message", otp);

        return true;
    }
}