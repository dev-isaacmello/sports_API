namespace csharp_api.Application.Dtos.Auth;

public class RegisterDto
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
}

