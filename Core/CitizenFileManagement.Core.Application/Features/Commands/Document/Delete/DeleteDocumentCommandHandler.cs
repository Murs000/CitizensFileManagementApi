using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Core.Domain.Entities;
using CitizenFileManagement.Infrastructure.External.Extensions;
using CitizenFileManagement.Infrastructure.External.Settings;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.IO;

namespace CitizenFileManagement.Core.Application.Features.Commands.Document.Delete
{
    public class DeleteDocumentCommandHandler : IRequestHandler<DeleteDocumentCommand, bool>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IUserManager _userManager;
        private readonly IUserRepository _userRepository;

        public DeleteDocumentCommandHandler (IDocumentRepository documentRepository, IUserRepository userRepository, IUserManager userManager)
        {
            _documentRepository = documentRepository;
            _userManager = userManager;
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
        {
            var userId = _userManager.GetCurrentUserId();

            var user = await _userRepository.GetAsync(u => u.Id == userId);

            foreach(var id in request.DeletedFiles)
            {
                var document = await _documentRepository.GetAsync(d => d.Id == id);

                File.Delete(document.Path);

                document.SetCredentials(user.Id);

                _documentRepository.Update(document);
            }

            await _documentRepository.SaveAsync();

            return true;
        }
    }
}