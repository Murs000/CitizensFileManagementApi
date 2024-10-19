using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Core.Domain.Entities;
using CitizenFileManagement.Infrastructure.External.Settings;
using CitizenFileManagement.Infrastructure.External.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.IO;
using CitizenFileManagement.Core.Domain.Enums;

namespace CitizenFileManagement.Core.Application.Features.Commands.FilePack.Update
{
    public class UpdateFilePackCommandHandler : IRequestHandler<UpdateFilePackCommand, bool>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IUserManager _userManager;
        private readonly IUserRepository _userRepository;
        private readonly FileSettings _fileSettings;
        private readonly IFilePackRepository _filePackRepository;

        public UpdateFilePackCommandHandler(IFilePackRepository filePackRepository, IDocumentRepository documentRepository, IUserRepository userRepository, IUserManager userManager, IOptions<FileSettings> fileSettings)
        {
            _filePackRepository = filePackRepository;
            _documentRepository = documentRepository;
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
                var file = await _documentRepository.GetAsync(uf => uf.Id == id);

                File.Delete(file.Path);

                file.SetCredentials(userId);

                _documentRepository.SoftDelete(file);
            }

            foreach(var pack in request.Files)
            {
                var filePack = await _filePackRepository.GetAsync(fp => fp.Id == pack.Id);

                filePack.Name = pack.Name;
                filePack.Description = pack.Description;

                if(pack.Files != null)
                {
                    foreach (var file in pack.Files)
                    {
                        string filePath = await file.SaveAsync(_fileSettings.Path, user.Username, pack.Name);

                        var document = new Domain.Entities.Document();

                        document.SetDetails(file.FileName, filePath, DocumentType.Other, null);
                        await _documentRepository.AddAsync(document);

                        filePack.Documents.Add(document);
                    }
                    await _documentRepository.SaveAsync();
                }
                filePack.SetCredentials(userId);

                _filePackRepository.Update(filePack);
            }
            await _filePackRepository.SaveAsync();

            return true;
        }
    }
}