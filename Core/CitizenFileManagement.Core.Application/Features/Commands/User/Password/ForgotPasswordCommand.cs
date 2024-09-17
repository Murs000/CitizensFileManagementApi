using MediatR;

namespace CitizenFileManagement.Core.Application.Features.Commands.User.Password;

public class ForgotPasswordCommand : IRequest<bool>
{
    public string Email { get; set; }
}
