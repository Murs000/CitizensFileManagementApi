using CitizenFileManagement.Core.Application.Common.Exceptions;
using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Infrastructure.External.Helpers;
using MediatR;

namespace CitizenFileManagement.Core.Application.Features.Commands.User.ConfirmEmail;

public class ResendConfirmEmailCommandHandler : IRequestHandler<ResendConfirmEmailCommand,bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;

    public ResendConfirmEmailCommandHandler(IUserRepository userRepository, IEmailService emailService)
    {
        _userRepository = userRepository;
        _emailService = emailService;
    }

    public async Task<bool> Handle(ResendConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(x => x.Email == request.Email && !x.IsActivated);

        if (user == null)
        {
            throw new NotFoundException("User not found or already activated.");
        }

        var newOtp = OTPHelper.GenerateOTP();
        user.OTP = newOtp;
        user.OTPExpireDate = DateTime.UtcNow.AddHours(4).AddMinutes(10);
        await _userRepository.SaveAsync();

        // Send confirmation email
        await _emailService.SendEmailAsync(user.Email,"OTP message", newOtp);

        return true;
    }
}