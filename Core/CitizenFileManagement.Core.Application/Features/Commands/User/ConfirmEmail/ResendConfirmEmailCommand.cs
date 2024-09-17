using MediatR;

namespace CitizenFileManagement.Core.Application.Features.Commands.User.ConfirmEmail;

public class ResendConfirmEmailCommand : IRequest<bool>
{
    public string Email { get; set; }
}