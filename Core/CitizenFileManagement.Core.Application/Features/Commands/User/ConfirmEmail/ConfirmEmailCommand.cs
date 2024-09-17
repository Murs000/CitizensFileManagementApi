using MediatR;

namespace CitizenFileManagement.Core.Application.Features.Commands.User.ConfirmEmail;

public class ConfirmEmailCommand : IRequest<bool>
{
    public string Email { get; set; }
    public string OTP { get; set; }
}
