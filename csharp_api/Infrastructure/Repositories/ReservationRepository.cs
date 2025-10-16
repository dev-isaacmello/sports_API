using csharp_api.Domain.Entities;
using csharp_api.Domain.Interfaces;
using csharp_api.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace csharp_api.Infrastructure.Repositories;

public class ReservationRepository(AppDbContext context) : IReservationRepository
{
    public async Task<IEnumerable<Reservation>> GetAllReservationsAsync()
    {
        return await context.Reservations
            .Include(r => r.User)
            .Include(r => r.Court)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<Reservation?> GetReservationByIdAsync(int id)
    {
        return await context.Reservations
            .Include(r => r.User)
            .Include(r => r.Court)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<Reservation>> GetReservationsByUserIdAsync(int userId)
    {
        return await context.Reservations
            .Include(r => r.Court)
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.StartTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetReservationsByCourtIdAsync(int courtId)
    {
        return await context.Reservations
            .Include(r => r.User)
            .Where(r => r.CourtId == courtId)
            .OrderByDescending(r => r.StartTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetReservationsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await context.Reservations
            .Include(r => r.User)
            .Include(r => r.Court)
            .Where(r => r.StartTime >= startDate && r.EndTime <= endDate)
            .OrderBy(r => r.StartTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetConflictingReservationsAsync(
        int courtId, 
        DateTime startTime, 
        DateTime endTime, 
        int? excludeReservationId = null)
    {
        var query = context.Reservations
            .Where(r => r.CourtId == courtId 
                && r.Status != "Cancelled"
                && ((r.StartTime < endTime && r.EndTime > startTime)));

        if (excludeReservationId.HasValue)
        {
            query = query.Where(r => r.Id != excludeReservationId.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<Reservation> CreateReservationAsync(Reservation reservation)
    {
        await context.Reservations.AddAsync(reservation);
        await context.SaveChangesAsync();
        
        // Recarregar com navegação
        return await GetReservationByIdAsync(reservation.Id) 
            ?? throw new InvalidOperationException("Erro ao criar reserva");
    }

    public async Task<Reservation> UpdateReservationAsync(int id, Reservation reservation)
    {
        var existingReservation = await context.Reservations.FindAsync(id)
            ?? throw new KeyNotFoundException($"Reserva com ID {id} não encontrada");
        
        reservation.Id = id;
        context.Entry(existingReservation).CurrentValues.SetValues(reservation);
        await context.SaveChangesAsync();
        
        return await GetReservationByIdAsync(id) 
            ?? throw new InvalidOperationException("Erro ao atualizar reserva");
    }

    public async Task<Reservation> CancelReservationAsync(int id, string? cancellationReason)
    {
        var reservation = await context.Reservations.FindAsync(id)
            ?? throw new KeyNotFoundException($"Reserva com ID {id} não encontrada");
        
        reservation.Status = "Cancelled";
        reservation.CancelledAt = DateTime.UtcNow;
        reservation.CancellationReason = cancellationReason;
        
        await context.SaveChangesAsync();
        
        return await GetReservationByIdAsync(id) 
            ?? throw new InvalidOperationException("Erro ao cancelar reserva");
    }

    public async Task DeleteReservationAsync(int id)
    {
        var reservation = await context.Reservations.FindAsync(id)
            ?? throw new KeyNotFoundException($"Reserva com ID {id} não encontrada");
        
        context.Reservations.Remove(reservation);
        await context.SaveChangesAsync();
    }
}

