using CitizenFileManagement.Core.Domain.Enums;
using MediatR;

namespace CitizenFileManagement.Core.Application.Features.Commands.User.Update;

public class UpdateUserCommand : IRequest<bool>
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public UserRole Role  {get; set; }
}