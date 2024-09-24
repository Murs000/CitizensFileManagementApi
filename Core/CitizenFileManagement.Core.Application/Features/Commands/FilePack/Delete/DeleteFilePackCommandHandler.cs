using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Core.Domain.Entities;
using CitizenFileManagement.Infrastructure.External.Settings;
using CitizenFileManagement.Infrastructure.External.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.IO;

namespace CitizenFileManagement.Core.Application.Features.Commands.FilePack.Delete
{
    public class DeleteFilePackCommandHandler : IRequestHandler<DeleteFilePackCommand, bool>
    {
        private readonly IUserFileRepository _userFileRepository;
        private readonly IUserManager _userManager;
        private readonly IUserRepository _userRepository;
        private readonly FileSettings _fileSettings;
        private readonly IFilePackRepository _filePackRepository;

        public DeleteFilePackCommandHandler(IFilePackRepository filePackRepository, IUserFileRepository userFileRepository, IUserRepository userRepository, IUserManager userManager, IOptions<FileSettings> fileSettings)
        {
            _filePackRepository = filePackRepository;
            _userFileRepository = userFileRepository;
            _userManager = userManager;
            _fileSettings = fileSettings.Value;
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(DeleteFilePackCommand request, CancellationToken cancellationToken)
        {
            var userId = _userManager.GetCurrentUserId();

            var user = await _userRepository.GetAsync(u => u.Id == userId);

            foreach (var id in request.FileIds)
            {
                var filePack = await _filePackRepository.GetAsync(uf => uf.Id == id);

                foreach(var file in filePack.Files)
                {
                    File.Delete(file.Path);

                    _userFileRepository.SoftDelete(file);

                    file.SetCredentials(userId);
                }
            
                _filePackRepository.SoftDelete(filePack);

                filePack.SetCredentials(userId);
            }

            await _filePackRepository.SaveAsync();

            return true;
        }
    }
}