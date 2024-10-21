using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Core.Domain.Entities;
using CitizenFileManagement.Infrastructure.External.Settings;
using CitizenFileManagement.Infrastructure.External.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.IO;
using CitizenFileManagement.Infrastructure.External.Services.MinIOService;

namespace CitizenFileManagement.Core.Application.Features.Commands.FilePack.Delete
{
    public class DeleteFilePackCommandHandler(IFilePackRepository filePackRepository,
        IDocumentRepository documentRepository, 
        IUserRepository userRepository, 
        IUserManager userManager,
        IMinIOService minIOService) : IRequestHandler<DeleteFilePackCommand, bool>
    {

        public async Task<bool> Handle(DeleteFilePackCommand request, CancellationToken cancellationToken)
        {
            var userId = userManager.GetCurrentUserId();

            var user = await userRepository.GetAsync(u => u.Id == userId, "FilePacks");

            foreach (int id in request.FileIds)
            {
                if (id == user.FilePacks.Select(f => f.Id).Min())
                    continue;

                var filePack = await filePackRepository.GetAsync(uf => uf.Id == id,"Documents");

                foreach(var file in filePack.Documents)
                {
                    await minIOService.DeleteFileAsync(file.Path, $"{user.Id}{user.Username}");

                    file.SetCredentials(userId);

                    documentRepository.SoftDelete(file);
                }
                filePack.SetCredentials(userId);
            
                filePackRepository.SoftDelete(filePack);
            }

            await filePackRepository.SaveAsync();

            return true;
        }
    }
}