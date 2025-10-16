using System.Security.Claims;
using csharp_api.Application.Dtos.Reservation;
using csharp_api.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace csharp_api.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReservationController(IReservationService reservationService) : ControllerBase
{
    /// <summary>
    /// Lista todas as reservas (apenas Admin)
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllReservations()
    {
        try
        {
            var reservations = await reservationService.GetAllReservationsAsync();
            return Ok(reservations);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao buscar reservas", error = ex.Message });
        }
    }

    /// <summary>
    /// Busca reserva por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetReservationById(int id)
    {
        try
        {
            var reservation = await reservationService.GetReservationByIdAsync(id);
            
            // Verifica se o usuário é admin ou dono da reserva
            var userId = GetUserIdFromToken();
            var userRole = GetUserRoleFromToken();
            
            if (userRole != "Admin" && reservation.UserId != userId)
            {
                return Forbid();
            }
            
            return Ok(reservation);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao buscar reserva", error = ex.Message });
        }
    }

    /// <summary>
    /// Lista reservas do usuário logado
    /// </summary>
    [HttpGet("my-reservations")]
    public async Task<IActionResult> GetMyReservations()
    {
        try
        {
            var userId = GetUserIdFromToken();
            var reservations = await reservationService.GetReservationsByUserIdAsync(userId);
            return Ok(reservations);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao buscar suas reservas", error = ex.Message });
        }
    }

    /// <summary>
    /// Lista reservas por quadra
    /// </summary>
    [HttpGet("court/{courtId}")]
    public async Task<IActionResult> GetReservationsByCourt(int courtId)
    {
        try
        {
            var reservations = await reservationService.GetReservationsByCourtIdAsync(courtId);
            return Ok(reservations);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao buscar reservas da quadra", error = ex.Message });
        }
    }

    /// <summary>
    /// Lista reservas por período
    /// </summary>
    [HttpGet("date-range")]
    public async Task<IActionResult> GetReservationsByDateRange(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        try
        {
            var reservations = await reservationService.GetReservationsByDateRangeAsync(startDate, endDate);
            return Ok(reservations);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao buscar reservas por período", error = ex.Message });
        }
    }

    /// <summary>
    /// Cria uma nova reserva
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateReservation([FromBody] CreateReservationDto createReservationDto)
    {
        try
        {
            var userId = GetUserIdFromToken();
            var reservation = await reservationService.CreateReservationAsync(userId, createReservationDto);
            return CreatedAtAction(nameof(GetReservationById), new { id = reservation.Id }, reservation);
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
            return StatusCode(500, new { message = "Erro ao criar reserva", error = ex.Message });
        }
    }

    /// <summary>
    /// Atualiza uma reserva
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateReservation(int id, [FromBody] UpdateReservationDto updateReservationDto)
    {
        try
        {
            var userId = GetUserIdFromToken();
            var reservation = await reservationService.UpdateReservationAsync(id, userId, updateReservationDto);
            return Ok(reservation);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao atualizar reserva", error = ex.Message });
        }
    }

    /// <summary>
    /// Cancela uma reserva
    /// </summary>
    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> CancelReservation(int id, [FromBody] CancelReservationDto cancelReservationDto)
    {
        try
        {
            var userId = GetUserIdFromToken();
            var reservation = await reservationService.CancelReservationAsync(id, userId, cancelReservationDto);
            return Ok(reservation);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao cancelar reserva", error = ex.Message });
        }
    }

    /// <summary>
    /// Deleta uma reserva (apenas Admin ou dono)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReservation(int id)
    {
        try
        {
            var userId = GetUserIdFromToken();
            var userRole = GetUserRoleFromToken();
            var isAdmin = userRole == "Admin";
            
            await reservationService.DeleteReservationAsync(id, userId, isAdmin);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao deletar reserva", error = ex.Message });
        }
    }

    // Métodos auxiliares para obter dados do token
    private int GetUserIdFromToken()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.Parse(userIdClaim ?? "0");
    }

    private string GetUserRoleFromToken()
    {
        return User.FindFirst(ClaimTypes.Role)?.Value ?? "User";
    }
}

