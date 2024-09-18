using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Core.Domain.Entities;
using CitizenFileManagement.Infrastructure.External.Settings;
using CitizenFileManagement.Infrastructure.External.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.IO;

namespace CitizenFileManagement.Core.Application.Features.Commands.Document.Create
{
    public class CreateDocumentCommandHandler : IRequestHandler<CreateDocumentCommand, bool>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IUserManager _userManager;
        private readonly IUserRepository _userRepository;
        private readonly FileSettings _fileSettings;

        public CreateDocumentCommandHandler(IDocumentRepository documentRepository, IUserRepository userRepository, IUserManager userManager,IOptions<FileSettings> fileSettings)
        {
            _documentRepository = documentRepository;
            _userManager = userManager;
            _fileSettings = fileSettings.Value;
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
        {
            var userId = _userManager.GetCurrentUserId();

            var user = await _userRepository.GetAsync(u => u.Id == userId);

            foreach (var documentDTO in request.Files)
            {
                string filePath = await documentDTO.File.SaveAsync(_fileSettings.Path , user.Username);

                var document = new Domain.Entities.Document
                {
                    Name = documentDTO.Name,
                    Path = filePath, 
                    Description = documentDTO.Description,
                    Type = documentDTO.Type,
                    CustomerId = userId
                };

                document.SetCreationCredentials(userId);

                await _documentRepository.AddAsync(document);
            }

            await _documentRepository.SaveAsync();

            return true;
        }
    }
}