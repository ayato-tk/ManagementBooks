using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Application.Requests.User.Commands;
using GestaoLivros.Domain.Interfaces;
using MediatR;

namespace GestaoLivros.Application.Requests.User.Handlers;

public class SignInUserCommandHandler(ITokenService tokenService, IUserRepository userRepository) : IRequestHandler<SignInUserCommand, string>
{
    public async Task<string> Handle(SignInUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);      
        
        var token = tokenService.GenerateToken(user!.Id.ToString());

        return token;
    }
}