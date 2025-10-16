using csharp_api.Domain.Entities;

namespace csharp_api.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(Users user);
    string GenerateRefreshToken();
}

