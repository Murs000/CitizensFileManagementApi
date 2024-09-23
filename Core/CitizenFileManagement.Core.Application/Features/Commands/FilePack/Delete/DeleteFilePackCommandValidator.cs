using FluentValidation;
using CitizenFileManagement.Core.Application.Features.Commands.FilePack.Create;

namespace CitizenFileManagement.Core.Application.Features.Commands.FilePack.Create;

public class DeleteFilePackCommandValidator : AbstractValidator<CreateFilePackCommand>
{
    public DeleteFilePackCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        RuleForEach(x => x.Files).NotNull();
    }
}