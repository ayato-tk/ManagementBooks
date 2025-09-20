using System.Security.Claims;
using GestaoLivros.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GestaoLivros.Application.Services.Implementations;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public long UserId
    {
        get
        {
            var userIdClaim = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)
                              ?? throw new UnauthorizedAccessException("Usuário não autenticado");

            return long.Parse(userIdClaim.Value);
        }
    }
}