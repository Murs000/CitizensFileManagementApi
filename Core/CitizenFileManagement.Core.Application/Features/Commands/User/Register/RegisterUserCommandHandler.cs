using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Core.Domain.Entities;
using CitizenFileManagement.Infrastructure.External.Helpers;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using CitizenFileManagement.Core.Domain.Enums;
using CitizenFileManagement.Infrastructure.External.Services.MinIOService;

namespace CitizenFileManagement.Core.Application.Features.Commands.User.Register
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IFilePackRepository _filePackRepository;
        private readonly IMinIOService _minIOService;

        public RegisterUserCommandHandler(IUserRepository userRepository, IEmailService emailService, IFilePackRepository filePackRepository, IMinIOService minIOService)
        {
            _filePackRepository = filePackRepository;
            _userRepository = userRepository;
            _emailService = emailService;
            _minIOService = minIOService;
        }

        public async Task<bool> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = new Domain.Entities.User();

            user.SetDetails(request.Username, request.Name, request.Surname, request.Email, UserRole.Personal)
                .SetCredentials(null);

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

            // Create Default Pack
            var filepack = new Domain.Entities.FilePack();
            filepack.SetDetails(request.Username,"All PackLess Files")
                .SetCredentials(user.Id)
                .SetUser(user.Id);
            await _filePackRepository.AddAsync(filepack);
            await _filePackRepository.SaveAsync();

            await _minIOService.EnsureBucketExistsAsync(user.Id+user.Username);

            // Send OTP email
            await _emailService.SendEmailAsync(user.Email, "OTP message", otp);

            return true;
        }
    }
}