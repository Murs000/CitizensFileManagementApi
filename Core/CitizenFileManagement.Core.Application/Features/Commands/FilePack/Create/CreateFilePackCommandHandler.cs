using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Core.Domain.Entities;
using CitizenFileManagement.Infrastructure.External.Settings;
using CitizenFileManagement.Infrastructure.External.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.IO;

namespace CitizenFileManagement.Core.Application.Features.Commands.FilePack.Create
{
    public class CreateFilePackCommandHandler : IRequestHandler<CreateFilePackCommand, bool>
    {
        private readonly IFilePackRepository _filePackRepository;
        private readonly IUserManager _userManager;
        private readonly IUserRepository _userRepository;
        private readonly FileSettings _fileSettings;

        public CreateFilePackCommandHandler(IFilePackRepository filePackRepository, IUserRepository userRepository, IUserManager userManager, IOptions<FileSettings> fileSettings)
        {
            _filePackRepository = filePackRepository;
            _userManager = userManager;
            _fileSettings = fileSettings.Value;
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(CreateFilePackCommand request, CancellationToken cancellationToken)
        {
            var userId = _userManager.GetCurrentUserId();

            var user = await _userRepository.GetAsync(u => u.Id == userId);

            foreach (var pack in request.FilePacks)
            {
                var filePack = new Domain.Entities.FilePack
                {
                    Name = pack.Name,
                    Description = pack.Description,
                    CustomerId = user.CustomerId,
                    Files = []
                };


                foreach (var file in pack.Files)
                {
                    string filePath = await file.SaveAsync(_fileSettings.Path+$"{pack.Name}", user.Username);

                    var userFile = new UserFile
                    {
                        Name = file.FileName,
                        Path = filePath
                    };

                    filePack.Files.Add(userFile);
                }

                filePack.SetCreationCredentials(userId);

                await _filePackRepository.AddAsync(filePack);
            }
            await _filePackRepository.SaveAsync();

            return true;
        }
    }
}