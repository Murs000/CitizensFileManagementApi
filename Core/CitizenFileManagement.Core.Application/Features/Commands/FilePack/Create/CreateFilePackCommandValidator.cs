using FluentValidation;
using CitizenFileManagement.Core.Application.Features.Commands.FilePack.Create;

namespace CitizenFileManagement.Core.Application.Features.Commands.FilePack.Create;

public class CreateFilePackCommandValidator : AbstractValidator<CreateFilePackCommand>
{
    public CreateFilePackCommandValidator()
    {
        RuleForEach(x => x.FilePacks)
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