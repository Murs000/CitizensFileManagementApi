using FluentValidation;
using CitizenFileManagement.Core.Application.Features.Commands.FilePack.Create;

namespace CitizenFileManagement.Core.Application.Features.Commands.FilePack.RemoveDocument;

public class RemoveDocumentFilePackCommandValidator : AbstractValidator<RemoveDocumentFilePackCommand>
{
    public RemoveDocumentFilePackCommandValidator()
    {
        RuleForEach(x => x.DocumentIds).NotEmpty();
        RuleFor(x => x.PackId).NotEmpty();
    }
}