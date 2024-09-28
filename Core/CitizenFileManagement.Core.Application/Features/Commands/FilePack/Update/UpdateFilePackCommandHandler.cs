using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Core.Domain.Entities;
using CitizenFileManagement.Infrastructure.External.Settings;
using CitizenFileManagement.Infrastructure.External.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.IO;

namespace CitizenFileManagement.Core.Application.Features.Commands.FilePack.Update
{
    public class UpdateFilePackCommandHandler : IRequestHandler<UpdateFilePackCommand, bool>
    {
        private readonly IUserFileRepository _userFileRepository;
        private readonly IUserManager _userManager;
        private readonly IUserRepository _userRepository;
        private readonly FileSettings _fileSettings;
        private readonly IFilePackRepository _filePackRepository;

        public UpdateFilePackCommandHandler(IFilePackRepository filePackRepository, IUserFileRepository userFileRepository, IUserRepository userRepository, IUserManager userManager, IOptions<FileSettings> fileSettings)
        {
            _filePackRepository = filePackRepository;
            _userFileRepository = userFileRepository;
            _userManager = userManager;
            _fileSettings = fileSettings.Value;
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(UpdateFilePackCommand request, CancellationToken cancellationToken)
        {
            var userId = _userManager.GetCurrentUserId();

            var user = await _userRepository.GetAsync(u => u.Id == userId);

            foreach (var id in request.FileIds)
            {
                var file = await _userFileRepository.GetAsync(uf => uf.Id == id);

                File.Delete(file.Path);

                _userFileRepository.SoftDelete(file);

                file.SetCredentials(userId);
            }

            foreach(var pack in request.Files)
            {
                var filePack = await _filePackRepository.GetAsync(fp => fp.Id == pack.Id);

                filePack.Name = pack.Name;
                filePack.Description = pack.Description;

                if(pack.Files != null)
                {
                    foreach(var file in pack.Files)
                    {
                        var userFile = new UserFile
                        {
                            Name = file.FileName,
                            Path = await file.SaveAsync(_fileSettings.Path+$"{pack.Name}", user.Username)        
                        };

                        filePack.Files.Add(userFile);
                    }
                }
                filePack.SetCredentials(userId);

                _filePackRepository.Update(filePack);
            }
            await _filePackRepository.SaveAsync();

            return true;
        }
    }
}