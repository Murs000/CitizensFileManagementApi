using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Core.Domain.Entities;
using CitizenFileManagement.Infrastructure.External.Helpers;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

namespace CitizenFileManagement.Core.Application.Features.Commands.User.Update
{
    public class RegisterUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserManager _userManager;

        public RegisterUserCommandHandler(IUserRepository userRepository,IUserManager userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var userId = _userManager.GetCurrentUserId();
            var user = await _userRepository.GetAsync(u => u.Id == userId,"Customer");

            user.Customer.Name = request.Name;
            user.Customer.Surname = request.Surname;

            _userRepository.Update(user);

            await _userRepository.SaveAsync();

            return true;
        }
    }
}