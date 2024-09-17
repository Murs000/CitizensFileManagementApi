using FluentValidation;

namespace CitizenFileManagement.Core.Application.Features.Commands.User.Login;

public class CreateAuthorizationTokenCommandValidator:AbstractValidator<CreateAuthorizationTokenCommand>
{
    public CreateAuthorizationTokenCommandValidator()
    {
        RuleFor(command => command.Username).NotNull();
        RuleFor(command => command.Password).MinimumLength(10).NotNull();
    }
}