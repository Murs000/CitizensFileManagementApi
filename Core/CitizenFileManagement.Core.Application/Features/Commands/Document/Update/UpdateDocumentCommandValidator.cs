using FluentValidation;
using CitizenFileManagement.Core.Application.Features.Commands.Document.Create;

namespace CitizenFileManagement.Core.Application.Features.Commands.Document.Update;

public class UpdateDocumentCommandValidator : AbstractValidator<UpdateDocumentCommand>
{
    public UpdateDocumentCommandValidator()
    {
        RuleForEach(x => x.documentDTOs)
            .ChildRules(documents =>
            {
                documents.RuleFor(x => x.Id)
                    .NotEmpty()
                    .WithMessage("Document id is required.");
                documents.RuleFor(x => x.Name)
                    .NotEmpty()
                    .WithMessage("Document name is required.");

                documents.RuleFor(x => x.Description)
                    .MaximumLength(500)
                    .WithMessage("Description must not exceed 500 characters.");

                documents.RuleFor(x => x.Type)
                    .IsInEnum()
                    .WithMessage("Invalid document type.");

                documents.RuleFor(x => x.File)
                    .NotNull()
                    .WithMessage("File is required");
            });
    }
}