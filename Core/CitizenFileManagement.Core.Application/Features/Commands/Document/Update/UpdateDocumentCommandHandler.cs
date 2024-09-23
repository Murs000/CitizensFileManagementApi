using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Core.Domain.Entities;
using CitizenFileManagement.Infrastructure.External.Extensions;
using CitizenFileManagement.Infrastructure.External.Settings;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.IO;

namespace CitizenFileManagement.Core.Application.Features.Commands.Document.Update
{
    public class UpdateDocumentCommandHandler : IRequestHandler<UpdateDocumentCommand, bool>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IUserManager _userManager;
        private readonly IUserRepository _userRepository;
        private readonly FileSettings _fileSettings;

        public UpdateDocumentCommandHandler(IDocumentRepository documentRepository, IUserRepository userRepository, IUserManager userManager, IOptions<FileSettings> fileSettings)
        {
            _documentRepository = documentRepository;
            _userManager = userManager;
            _fileSettings = fileSettings.Value;
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(UpdateDocumentCommand request, CancellationToken cancellationToken)
        {
            var userId = _userManager.GetCurrentUserId();

            var user = await _userRepository.GetAsync(u => u.Id == userId);

            foreach (var file in request.Files)
            {
                var document = await _documentRepository.GetAsync(d => d.Id == file.Id);

                document.Name = file.Name;
                document.Description = file.Description;
                document.Type = file.Type;

                if (file.File != null)
                {
                    File.Delete(document.Path);

                    var filePath = await file.File.SaveAsync(_fileSettings.Path, user.Username);

                    document.Path = filePath;
                }

                document.SetCredentials(user.Id);

                _documentRepository.Update(document);
            }

            await _documentRepository.SaveAsync();

            return true;
        }
    }
}