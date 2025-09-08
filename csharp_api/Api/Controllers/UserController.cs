using AutoMapper;
using csharp_api.Application.Dtos;
using csharp_api.Application.Interfaces;
using csharp_api.Application.Validation;
using csharp_api.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace csharp_api.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService, IMapper mapper, UserDtoValidator validator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllUsersAsync()
    {
        var users = await userService.GetAllUsersAsync();
        return Ok(mapper.Map<IEnumerable<UserDto>>(users));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserByIdAsync(int id)
    {
        var user = await userService.GetUserByIdAsync(id);
        return Ok(mapper.Map<UserDto>(user));
    }

    [HttpPost]
    public async Task<IActionResult> CreateUserAsync([FromBody] UserDto userDto)
    {
        var newUser = mapper.Map<Users>(userDto);
        // Validação do DTO via FluentValidation
        var validationResult = await validator.ValidateAsync(userDto);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
        
        var createdUser = await userService.InserUserAsync(newUser);
        
        return Ok(mapper.Map<Users>(createdUser));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUserAsync(int id,[FromBody] UserDto userDto)
    {
        var user = mapper.Map<Users>(userDto);
        var validationResult = await validator.ValidateAsync(userDto);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
        await userService.UpdateUserAsync(id, user);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserAsync(int id)
    {
        await userService.DeleteAsync(id);
        return NoContent();
    }
    
}