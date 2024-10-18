using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Core.Domain.Entities;
using CitizenFileManagement.Infrastructure.External.Helpers;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using CitizenFileManagement.Core.Domain.Enums;

namespace CitizenFileManagement.Core.Application.Features.Commands.User.Register
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public RegisterUserCommandHandler(IUserRepository userRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public async Task<bool> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = new Domain.Entities.User
            {
                Username = request.Username,
                Name = request.Name,
                Surname = request.Surname,
                Email = request.Email,
                IsActivated = false,
            };

            user.SetDetails(request.Username, request.Name, request.Surname, request.Email, UserRole.Personal);

            // Hash password
            (string hash, string salt) = PasswordHelper.HashPassword(request.Password);
            user.SetPassword(hash, salt);

            // Generate OTP
            var otp = OTPHelper.GenerateOTP();
            user.OTP = otp;
            user.OTPExpireDate = DateTime.UtcNow.AddHours(4).AddMinutes(10);

            // Save user and customer data
            await _userRepository.AddAsync(user);
            await _userRepository.SaveAsync();

            // Send OTP email
            await _emailService.SendEmailAsync(user.Email, "OTP message", otp);

            return true;
        }
    }
}