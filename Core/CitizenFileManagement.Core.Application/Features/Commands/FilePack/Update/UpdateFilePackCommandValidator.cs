using FluentValidation;
using CitizenFileManagement.Core.Application.Features.Commands.FilePack.Create;

namespace CitizenFileManagement.Core.Application.Features.Commands.FilePack.Update;

public class UpdateFilePackCommandValidator : AbstractValidator<UpdateFilePackCommand>
{
    public UpdateFilePackCommandValidator()
    {
        RuleForEach(x => x.FileIds).NotNull();
        RuleForEach(x => x.Files)
            .ChildRules(documents =>
            {
                documents.RuleFor(x => x.Name)
                    .NotEmpty()
                    .WithMessage("Document name is required.");

                documents.RuleFor(x => x.Description)
                    .MaximumLength(500)
                    .WithMessage("Description must not exceed 500 characters.");

                documents.RuleForEach(x => x.Files);
            });
    }
}