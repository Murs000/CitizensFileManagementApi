using FluentValidation;
using CitizenFileManagement.Core.Application.Features.Commands.FilePack.Create;

namespace CitizenFileManagement.Core.Application.Features.Commands.FilePack.Delete;

public class DeleteFilePackCommandValidator : AbstractValidator<DeleteFilePackCommand>
{
    public DeleteFilePackCommandValidator()
    {
        RuleForEach(x => x.FileIds).NotNull();
    }
}