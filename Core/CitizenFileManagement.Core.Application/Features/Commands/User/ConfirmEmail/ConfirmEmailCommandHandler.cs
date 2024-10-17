using CitizenFileManagement.Core.Application.Common.Exceptions;
using CitizenFileManagement.Core.Application.Interfaces;
using MediatR;

namespace CitizenFileManagement.Core.Application.Features.Commands.User.ConfirmEmail;

public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand,bool>
{
    private readonly IUserRepository _userRepository;

    public ConfirmEmailCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(x => x.Email == request.Email && x.OTP == request.OTP && x.OTPExpireDate > DateTime.UtcNow.AddHours(4));

        if (user == null || user.IsActivated)
        {
            throw new NotFoundException("Invalid OTP or user already activated.");
        }

        user.IsActivated = true;
        user.OTP = null; // clear OTP after successful confirmation
        user.OTPExpireDate = null;

        await _userRepository.SaveAsync();

        return true;
    }
}
