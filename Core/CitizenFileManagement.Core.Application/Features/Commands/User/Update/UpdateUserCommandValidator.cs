using FluentValidation;

namespace CitizenFileManagement.Core.Application.Features.Commands.User.Update;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Surname).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Role).IsInEnum();
    }
}