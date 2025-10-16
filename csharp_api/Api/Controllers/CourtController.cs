using csharp_api.Application.Dtos.Court;
using csharp_api.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace csharp_api.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourtController(ICourtService courtService) : ControllerBase
{
    /// <summary>
    /// Lista todas as quadras
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllCourts()
    {
        try
        {
            var courts = await courtService.GetAllCourtsAsync();
            return Ok(courts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao buscar quadras", error = ex.Message });
        }
    }

    /// <summary>
    /// Lista apenas quadras ativas
    /// </summary>
    [HttpGet("active")]
    public async Task<IActionResult> GetActiveCourts()
    {
        try
        {
            var courts = await courtService.GetActiveCourtsAsync();
            return Ok(courts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao buscar quadras ativas", error = ex.Message });
        }
    }

    /// <summary>
    /// Busca quadra por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCourtById(int id)
    {
        try
        {
            var court = await courtService.GetCourtByIdAsync(id);
            return Ok(court);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao buscar quadra", error = ex.Message });
        }
    }

    /// <summary>
    /// Busca quadras por tipo
    /// </summary>
    [HttpGet("type/{type}")]
    public async Task<IActionResult> GetCourtsByType(string type)
    {
        try
        {
            var courts = await courtService.GetCourtsByTypeAsync(type);
            return Ok(courts);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao buscar quadras por tipo", error = ex.Message });
        }
    }

    /// <summary>
    /// Cria uma nova quadra (apenas Admin)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateCourt([FromBody] CreateCourtDto createCourtDto)
    {
        try
        {
            var court = await courtService.CreateCourtAsync(createCourtDto);
            return CreatedAtAction(nameof(GetCourtById), new { id = court.Id }, court);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao criar quadra", error = ex.Message });
        }
    }

    /// <summary>
    /// Atualiza uma quadra (apenas Admin)
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateCourt(int id, [FromBody] UpdateCourtDto updateCourtDto)
    {
        try
        {
            var court = await courtService.UpdateCourtAsync(id, updateCourtDto);
            return Ok(court);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao atualizar quadra", error = ex.Message });
        }
    }

    /// <summary>
    /// Deleta uma quadra (apenas Admin)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCourt(int id)
    {
        try
        {
            await courtService.DeleteCourtAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao deletar quadra", error = ex.Message });
        }
    }
}

