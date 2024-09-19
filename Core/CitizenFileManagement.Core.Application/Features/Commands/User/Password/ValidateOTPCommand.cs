using MediatR;

namespace CitizenFileManagement.Core.Application.Features.Commands.User.Password;

public class ValidateOTPCommand : IRequest<bool>
{
    public string OTP { get; set; }
}