using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Core.Domain.Entities;
using CitizenFileManagement.Infrastructure.External.Extensions;
using CitizenFileManagement.Infrastructure.External.Services.MinIOService;
using CitizenFileManagement.Infrastructure.External.Settings;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.IO;

namespace CitizenFileManagement.Core.Application.Features.Commands.Document.Delete
{
    public class DeleteDocumentCommandHandler(IDocumentRepository documentRepository, 
        IUserRepository userRepository, 
        IUserManager userManager,
        IMinIOService minIOService) : IRequestHandler<DeleteDocumentCommand, bool>
    {
        public async Task<bool> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
        {
            var userId = userManager.GetCurrentUserId();

            var user = await userRepository.GetAsync(u => u.Id == userId);

            foreach(var id in request.DeletedFiles)
            {
                var document = await documentRepository.GetAsync(d => d.Id == id);

                await minIOService.DeleteFileAsync(document.Path, $"{user.Id}{user.Username}");

                document.SetCredentials(user.Id);

                documentRepository.SoftDelete(document);
            }

            await documentRepository.SaveAsync();

            return true;
        }
    }
}