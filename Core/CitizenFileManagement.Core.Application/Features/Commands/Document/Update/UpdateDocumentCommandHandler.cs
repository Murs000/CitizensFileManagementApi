using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Core.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace CitizenFileManagement.Core.Application.Features.Commands.Document.Update
{
    public class UpdateDocumentCommandHandler : IRequestHandler<UpdateDocumentCommand, bool>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IUserManager _userManager;
        private readonly string _storagePath = "/Users/mursal/Projects/CitizensFileManagementApi/Files"; // Directory to store files

        public UpdateDocumentCommandHandler(IDocumentRepository documentRepository, IUserManager userManager)
        {
            _documentRepository = documentRepository;
            _userManager = userManager;
        }

        public async Task<bool> Handle(UpdateDocumentCommand request, CancellationToken cancellationToken)
        {
            var userId = _userManager.GetCurrentUserId();

            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }

            foreach (var file in request.AddedFiles)
            {
                var fileName = $"{Guid.NewGuid()}_{file.Name}";
                var filePath = Path.Combine(_storagePath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.File.CopyToAsync(stream);
                }

                var document = new Domain.Entities.Document
                {
                    Name = file.Name,
                    Path = filePath,  
                    Description = file.Description,
                    Type = file.Type,
                    CustomerId = userId
                };

                document.SetCreationCredentials(userId);

                await _documentRepository.AddAsync(document);
            }
            foreach(var deletedId in request.DeletedId)
            {
                var file = await _documentRepository.GetAsync(d => d.Id == deletedId);

                File.Delete(file.Path);

                _documentRepository.SoftDelete(file);
            }

            await _documentRepository.SaveAsync();

            return true;
        }
    }
}