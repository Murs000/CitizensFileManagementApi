using MediatR;

namespace CitizenFileManagement.Core.Application.Features.Commands.User.Update;

public class UpdateUserCommand : IRequest<bool>
{
    public string Name { get; set; }
    public string Surname { get; set; }
}