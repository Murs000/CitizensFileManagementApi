using CitizenFileManagement.Core.Application.Features.Commands.User.ViewModels;
using MediatR;

namespace CitizenFileManagement.Core.Application.Features.Commands.User.Login;

public class CreateAuthorizationTokenCommand:IRequest<JwtTokenDto>
    {
        public string Username {  get; set; }
        public string Password { get; set; }    
    }
