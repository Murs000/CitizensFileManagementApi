using MediatR;

namespace CitizenFileManagement.Core.Application.Features.Commands.User.Password;

public class ResetPasswordCommand : IRequest<bool>
{
    public string Email { get; set; }
    public string OTP { get; set; }
    public string NewPassword { get; set; }
}
