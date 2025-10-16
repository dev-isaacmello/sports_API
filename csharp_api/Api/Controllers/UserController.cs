using AutoMapper;
using csharp_api.Application.Dtos;
using csharp_api.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace csharp_api.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController(IUserService userService, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Lista todos os usuários (apenas Admin)
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllUsersAsync()
    {
        try
        {
            var users = await userService.GetAllUsersAsync();
            return Ok(mapper.Map<IEnumerable<UserDto>>(users));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao buscar usuários", error = ex.Message });
        }
    }

    /// <summary>
    /// Busca usuário por ID (apenas Admin)
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUserByIdAsync(int id)
    {
        try
        {
            var user = await userService.GetUserByIdAsync(id);
            return Ok(mapper.Map<UserDto>(user));
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Deleta usuário (apenas Admin)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUserAsync(int id)
    {
        try
        {
            await userService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}