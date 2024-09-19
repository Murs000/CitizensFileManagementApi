using CitizenFileManagement.Core.Application.Exceptions;
using CitizenFileManagement.Core.Application.Interfaces;
using MediatR;

namespace CitizenFileManagement.Core.Application.Features.Commands.User.Password;

public class ValidateOTPCommandHandler : IRequestHandler<ValidateOTPCommand, bool>
{
    private readonly IUserRepository _userRepository;

    public ValidateOTPCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(ValidateOTPCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(x => x.OTP == request.OTP && x.OTPExpireDate < DateTime.UtcNow.AddHours(4));

        if (user == null)
        {
            throw new NotFoundException("Invalid OTP.");
        }

        return true;
    }
}