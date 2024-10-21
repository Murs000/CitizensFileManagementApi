using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Core.Domain.Entities;
using CitizenFileManagement.Infrastructure.External.Settings;
using CitizenFileManagement.Infrastructure.External.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.IO;
using CitizenFileManagement.Infrastructure.External.Services.MinIOService;

namespace CitizenFileManagement.Core.Application.Features.Commands.Document.Create
{
    public class CreateDocumentCommandHandler(IDocumentRepository documentRepository, 
        IUserRepository userRepository, 
        IUserManager userManager,
        IMinIOService minIOService) : IRequestHandler<CreateDocumentCommand, bool>
    {
        // depricated
        // private readonly FileSettings _fileSettings;

        public async Task<bool> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
        {
            var userId = userManager.GetCurrentUserId();

            var user = await userRepository.GetAsync(u => u.Id == userId, "FilePacks");

            // TODO: Add User Nullable Check

            foreach (var documentDTO in request.Files)
            {
                // depricated code
                // string filePath = await documentDTO.File.SaveAsync(_fileSettings.Path , user.Username, null);

                // Get the file stream and metadata from the uploaded file
                var file = documentDTO.File;
                var fileName = $"{documentDTO.Name}_{Guid.NewGuid()}";
                var bucketName = $"{user.Id}{user.Username}";

                // Upload the file to MinIO using the service
                using (var fileStream = file.OpenReadStream())
                {
                    await minIOService.UploadFileAsync(fileName, fileStream, file.ContentType, bucketName);
                }

                var document = new Domain.Entities.Document();

                document.SetDetails(documentDTO.Name, fileName, documentDTO.Type, file.ContentType, documentDTO.Description)
                    .SetCredentials(userId)
                    .SetFilePack(user.FilePacks.Select(fp => fp.Id).Min());

                await documentRepository.AddAsync(document);
            }

            await documentRepository.SaveAsync();

            return true;
        }
    }
}