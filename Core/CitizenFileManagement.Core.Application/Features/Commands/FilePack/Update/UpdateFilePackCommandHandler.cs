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

namespace CitizenFileManagement.Core.Application.Features.Commands.FilePack.Update
{
    public class UpdateFilePackCommandHandler(IFilePackRepository filePackRepository, 
        IDocumentRepository documentRepository, 
        IUserRepository userRepository, 
        IUserManager userManager,
        IMinIOService minIOService) : IRequestHandler<UpdateFilePackCommand, bool>
    {
        public async Task<bool> Handle(UpdateFilePackCommand request, CancellationToken cancellationToken)
        {
            var userId = userManager.GetCurrentUserId();

            var user = await userRepository.GetAsync(u => u.Id == userId);

            foreach (var id in request.FileIds)
            {
                var file = await documentRepository.GetAsync(uf => uf.Id == id);

                await minIOService.DeleteFileAsync(file.Path,$"{user.Id}{user.Username}");

                file.SetCredentials(userId);

                documentRepository.SoftDelete(file);
            }

            foreach(var pack in request.Files)
            {
                var filePack = await filePackRepository.GetAsync(fp => fp.Id == pack.Id);

                filePack.Name = pack.Name;
                filePack.Description = pack.Description;

                if(pack.Files != null)
                {
                    foreach (var file in pack.Files)
                    {
                        // Get the file stream and metadata from the uploaded file
                        var fileName = $"{pack.Name}/{file.FileName}";
                        var bucketName = $"{user.Id}{user.Username}";

                        // Upload the file to MinIO using the service
                        using (var fileStream = file.OpenReadStream())
                        {
                            await minIOService.UploadFileAsync(fileName, fileStream, file.ContentType, bucketName);
                        }

                        var document = new Domain.Entities.Document();

                        document.SetDetails(file.FileName, fileName, DocumentType.Other, file.ContentType, null);
                        await documentRepository.AddAsync(document);

                        filePack.Documents.Add(document);
                    }
                    await documentRepository.SaveAsync();
                }
                filePack.SetCredentials(userId);

                filePackRepository.Update(filePack);
            }
            await filePackRepository.SaveAsync();

            return true;
        }
    }
}