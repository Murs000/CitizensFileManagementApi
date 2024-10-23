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

namespace CitizenFileManagement.Core.Application.Features.Commands.FilePack.RemoveDocument
{
    public class RemoveDocumentFilePackCommandHandler(IFilePackRepository filePackRepository,
        IDocumentRepository documentRepository, 
        IUserRepository userRepository, 
        IUserManager userManager, 
        IMinIOService minIOService) : IRequestHandler<RemoveDocumentFilePackCommand, bool>
    {
        public async Task<bool> Handle(RemoveDocumentFilePackCommand request, CancellationToken cancellationToken)
        {
            var userId = userManager.GetCurrentUserId();

            var user = await userRepository.GetAsync(u => u.Id == userId);

            var filePack = await filePackRepository.GetAsync(fp => fp.Id == request.PackId, "Documents");

            var documents = await documentRepository.GetAllAsync(d => request.DocumentIds.Contains(d.Id) && d.FilePackId == request.PackId);

            foreach (var document in documents)
            {
                document.SetFilePack(request.PackId)
                    .SetCredentials(userId);

                var index = document.Path.IndexOf('/');
                var newPath = string.Empty;
                if (index != -1)
                {
                    newPath = document.Path.Substring(index + 1);
                }
                document.Path = newPath;
                document.Description = $"Dcument from {filePack.Name}";

                documentRepository.Update(document);
                await documentRepository.SaveAsync();
                
                await minIOService.MoveFileAsync(document.Path, $"{user.Id}{user.Username}", newPath);
            }

            return true;
        }
    }
}