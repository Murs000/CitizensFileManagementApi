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
        private readonly IUserManager _userManager;
        private readonly IUserRepository _userRepository;
        private readonly FileSettings _fileSettings;
        private readonly IFilePackRepository _filePackRepository;
        private readonly IDocumentRepository _documentRepository;

        public DeleteFilePackCommandHandler(IFilePackRepository filePackRepository,IDocumentRepository documentRepository, IUserRepository userRepository, IUserManager userManager, IOptions<FileSettings> fileSettings)
        {
            _filePackRepository = filePackRepository;
            _documentRepository = documentRepository;
            _userManager = userManager;
            _fileSettings = fileSettings.Value;
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(DeleteFilePackCommand request, CancellationToken cancellationToken)
        {
            var userId = _userManager.GetCurrentUserId();

            var user = await _userRepository.GetAsync(u => u.Id == userId, "FilePacks");

            foreach (int id in request.FileIds)
            {
                if (id == user.FilePacks.Select(f => f.Id).Min())
                    continue;

                var filePack = await _filePackRepository.GetAsync(uf => uf.Id == id,"Documents");

                foreach(var file in filePack.Documents)
                {
                    File.Delete(file.Path);

                    file.SetCredentials(userId);

                    _documentRepository.SoftDelete(file);
                }
                filePack.SetCredentials(userId);
            
                _filePackRepository.SoftDelete(filePack);
            }

            await _filePackRepository.SaveAsync();

            return true;
        }
    }
}