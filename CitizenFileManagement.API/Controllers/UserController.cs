using CitizenFileManagement.Core.Application.Features.Commands.User.Login;
using CitizenFileManagement.Core.Application.Features.Commands.User.Register;
using CitizenFileManagement.Core.Application.Features.Commands.User.RefreshToken;
using CitizenFileManagement.Core.Application.Features.Commands.User.ConfirmEmail;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using CitizenFileManagement.Core.Application.Features.Commands.User.Password;

namespace CitizenFileManagement.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(CreateAuthorizationTokenCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost("resend-confirm-email")]
        public async Task<IActionResult> ResendConfirmEmail(ResendConfirmEmailCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        [HttpPost("confirm-otp")]
        public async Task<IActionResult> ConfirmOTP(ValidateOTPCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}