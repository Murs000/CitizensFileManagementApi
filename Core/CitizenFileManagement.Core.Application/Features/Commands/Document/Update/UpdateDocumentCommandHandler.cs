using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Core.Domain.Entities;
using CitizenFileManagement.Infrastructure.External.Extensions;
using CitizenFileManagement.Infrastructure.External.Services.MinIOService;
using CitizenFileManagement.Infrastructure.External.Settings;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.IO;

namespace CitizenFileManagement.Core.Application.Features.Commands.Document.Update
{
    public class UpdateDocumentCommandHandler(IDocumentRepository documentRepository, 
        IUserRepository userRepository, 
        IUserManager userManager, 
        IMinIOService minIOService) : IRequestHandler<UpdateDocumentCommand, bool>
    {

        public async Task<bool> Handle(UpdateDocumentCommand request, CancellationToken cancellationToken)
        {
            var userId = userManager.GetCurrentUserId();

            var user = await userRepository.GetAsync(u => u.Id == userId);

            foreach (var documentDTO in request.documentDTOs)
            {
                var document = await documentRepository.GetAsync(d => d.Id == documentDTO.Id);

                string documentPath = string.Empty;
                string contentType = string.Empty;

                if (documentDTO.File != null)
                {
                    // Get the file stream and metadata from the uploaded file
                    var file = documentDTO.File;
                    var fileName = $"{documentDTO.Name}";
                    var bucketName = $"{user.Id}{user.Username}";
                    contentType = file.ContentType;

                    await minIOService.DeleteFileAsync(document.Path, bucketName);

                    // Upload the file to MinIO using the service
                    using (var fileStream = file.OpenReadStream())
                    {
                        await minIOService.UploadFileAsync(fileName, fileStream, contentType, bucketName);
                    }

                    

                    documentPath = fileName;
                }
                document.SetDetails(documentDTO.Name, documentPath, documentDTO.Type, contentType, documentDTO.Description);

                document.SetCredentials(user.Id);

                documentRepository.Update(document);
            }

            await documentRepository.SaveAsync();

            return true;
        }
    }
}