using csharp_api.Application.Dtos.Auth;
using csharp_api.Application.Interfaces;
using csharp_api.Domain.Entities;
using csharp_api.Domain.Interfaces;
using BCrypt.Net;

namespace csharp_api.Application.Services;

public class AuthService(IUserRepository userRepository, ITokenService tokenService) : IAuthService
{
    public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
    {
        // Verificar se o email já existe
        var existingUser = await userRepository.GetUserByEmailAsync(registerDto.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Email já cadastrado");
        }

        // Criar novo usuário
        var user = new Users
        {
            Nome = registerDto.Nome,
            Email = registerDto.Email,
            Telefone = registerDto.Telefone,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
            Role = "User",
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        var createdUser = await userRepository.InsertUserAsync(user);
        var token = tokenService.GenerateToken(createdUser);

        return new AuthResponseDto
        {
            Token = token,
            Email = createdUser.Email,
            Nome = createdUser.Nome,
            Role = createdUser.Role,
            ExpiresAt = DateTime.UtcNow.AddHours(8)
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        var user = await userRepository.GetUserByEmailAsync(loginDto.Email);
        
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Email ou senha inválidos");
        }

        if (!user.IsActive)
        {
            throw new UnauthorizedAccessException("Usuário inativo");
        }

        var token = tokenService.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            Email = user.Email,
            Nome = user.Nome,
            Role = user.Role,
            ExpiresAt = DateTime.UtcNow.AddHours(8)
        };
    }
}

