using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Core.Domain.Entities;
using CitizenFileManagement.Infrastructure.External.Settings;
using CitizenFileManagement.Infrastructure.External.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.IO;
using CitizenFileManagement.Core.Domain.Enums;
using CitizenFileManagement.Infrastructure.External.Services.MinIOService;

namespace CitizenFileManagement.Core.Application.Features.Commands.FilePack.Create
{
    public class CreateFilePackCommandHandler(IFilePackRepository filePackRepository,
        IDocumentRepository documentRepository, 
        IUserRepository userRepository, 
        IUserManager userManager, 
        IMinIOService minIOService) : IRequestHandler<CreateFilePackCommand, bool>
    {
        // depricated code
        // private readonly FileSettings _fileSettings;

        public async Task<bool> Handle(CreateFilePackCommand request, CancellationToken cancellationToken)
        {
            var userId = userManager.GetCurrentUserId();

            var user = await userRepository.GetAsync(u => u.Id == userId);

            foreach (var pack in request.FilePacks)
            {
                var filePack = new Domain.Entities.FilePack();

                filePack.SetDetails(pack.Name, pack.Description)
                    .SetCreationCredentials(userId)
                    .SetUser(userId);
                filePack.Documents = [];

                await filePackRepository.AddAsync(filePack);
                await filePackRepository.SaveAsync();

                if(pack.Files != null)
                {
                    foreach (var file in pack.Files)
                    {
                        // depricated code
                        // string filePath = await file.SaveAsync(_fileSettings.Path, user.Username, pack.Name);

                        // Get the file stream and metadata from the uploaded file
                        var fileName = $"{pack.Name}/{file.FileName}_{Guid.NewGuid()}";
                        var bucketName = $"{user.Id}{user.Username}";

                        // Upload the file to MinIO using the service
                        using (var fileStream = file.OpenReadStream())
                        {
                            await minIOService.UploadFileAsync(fileName, fileStream, file.ContentType, bucketName);
                        }

                        var document = new Domain.Entities.Document();

                        document.SetDetails(file.FileName, fileName, DocumentType.Other, file.ContentType, null)
                            .SetFilePack(filePack.Id)
                            .SetCredentials(userId);

                        await documentRepository.AddAsync(document);
                    }
                    await documentRepository.SaveAsync();
                }
            }

            return true;
        }
    }
}