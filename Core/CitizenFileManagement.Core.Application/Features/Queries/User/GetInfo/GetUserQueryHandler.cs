using MediatR;
using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Core.Application.Common.Exceptions;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using CitizenFileManagement.Core.Application.Features.Queries.User.ViewModels;

namespace CitizenFileManagement.Core.Application.Features.Queries.User.GetInfo;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserManager _userManager;

    public GetUserQueryHandler(IUserRepository userRepository, IUserManager userManager)
    {
        _userRepository = userRepository;
        _userManager = userManager;
    }

    public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        // Extract email or user ID from the token (claims)
        var userId = _userManager.GetCurrentUserId();
        var user = await _userRepository.GetAsync(u => u.Id == userId,"Customer");

        if (user == null)
        {
            throw new NotFoundException("User not found.");
        }

        // Map entity to DTO
        var userDto = new UserDto
        {
            Username = user.Username,
            Name = user.Customer.Name,
            Surname = user.Customer.Surname,
            Email = user.Email
        };

        return userDto;
    }
}