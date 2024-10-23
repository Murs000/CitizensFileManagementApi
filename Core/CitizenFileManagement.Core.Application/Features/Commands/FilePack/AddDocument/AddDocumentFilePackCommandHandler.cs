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

namespace CitizenFileManagement.Core.Application.Features.Commands.FilePack.AddDocument
{
    public class AddDocumentFilePackCommandHandler(IFilePackRepository filePackRepository,
        IDocumentRepository documentRepository, 
        IUserRepository userRepository, 
        IUserManager userManager, 
        IMinIOService minIOService) : IRequestHandler<AddDocumentFilePackCommand, bool>
    {
        public async Task<bool> Handle(AddDocumentFilePackCommand request, CancellationToken cancellationToken)
        {
            var userId = userManager.GetCurrentUserId();

            var user = await userRepository.GetAsync(u => u.Id == userId);

            var filePack = await filePackRepository.GetAsync(fp => fp.Id == request.PackId);

            var documents = await documentRepository.GetAllAsync(d => request.DocumentIds.Contains(d.Id));

            foreach (var document in documents)
            {
                document.SetFilePack(request.PackId)
                    .SetCredentials(userId);

                var newPath = $"{filePack.Name}/{document.Path}";
                document.Path = newPath;

                documentRepository.Update(document);
                await documentRepository.SaveAsync();
                
                await minIOService.MoveFileAsync(document.Path, $"{user.Id}{user.Username}", newPath);
            }

            return true;
        }
    }
}