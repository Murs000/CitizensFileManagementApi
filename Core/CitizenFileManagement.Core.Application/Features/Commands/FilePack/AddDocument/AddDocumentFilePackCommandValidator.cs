using FluentValidation;
using CitizenFileManagement.Core.Application.Features.Commands.FilePack.Create;

namespace CitizenFileManagement.Core.Application.Features.Commands.FilePack.AddDocument;

public class AddDocumentFilePackCommandValidator : AbstractValidator<AddDocumentFilePackCommand>
{
    public AddDocumentFilePackCommandValidator()
    {
        RuleForEach(x => x.DocumentIds).NotEmpty();
        RuleFor(x => x.PackId).NotEmpty();
    }
}