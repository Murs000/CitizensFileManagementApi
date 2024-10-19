using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Core.Domain.Entities;
using CitizenFileManagement.Infrastructure.External.Settings;
using CitizenFileManagement.Infrastructure.External.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.IO;
using CitizenFileManagement.Core.Domain.Enums;

namespace CitizenFileManagement.Core.Application.Features.Commands.FilePack.Create
{
    public class CreateFilePackCommandHandler : IRequestHandler<CreateFilePackCommand, bool>
    {
        private readonly IFilePackRepository _filePackRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IUserManager _userManager;
        private readonly IUserRepository _userRepository;
        private readonly FileSettings _fileSettings;

        public CreateFilePackCommandHandler(IFilePackRepository filePackRepository,IDocumentRepository documentRepository, IUserRepository userRepository, IUserManager userManager, IOptions<FileSettings> fileSettings)
        {
            _filePackRepository = filePackRepository;
            _documentRepository = documentRepository;
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
                var filePack = new Domain.Entities.FilePack();

                filePack.SetDetails(pack.Name, pack.Description)
                    .SetCreationCredentials(userId)
                    .SetUser(userId);
                filePack.Documents = [];

                await _filePackRepository.AddAsync(filePack);
                await _filePackRepository.SaveAsync();

                if(pack.Files != null)
                {
                    foreach (var file in pack.Files)
                    {
                        string filePath = await file.SaveAsync(_fileSettings.Path, user.Username, pack.Name);

                        var document = new Domain.Entities.Document();

                        document.SetDetails(file.FileName, filePath, DocumentType.Other, null)
                            .SetFilePack(filePack.Id)
                            .SetCredentials(userId);

                        await _documentRepository.AddAsync(document);
                    }
                    await _documentRepository.SaveAsync();
                }
            }

            return true;
        }
    }
}