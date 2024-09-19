using CitizenFileManagement.Core.Application.Features.Queries.User.ViewModels;
using MediatR;

namespace CitizenFileManagement.Core.Application.Features.Queries.User.GetInfo;

public class GetUserQuery : IRequest<UserDto>
{
}