using csharp_api.Domain.Entities;

namespace csharp_api.Domain.Interfaces;

public interface IReservationRepository
{
    Task<IEnumerable<Reservation>> GetAllReservationsAsync();
    Task<Reservation?> GetReservationByIdAsync(int id);
    Task<IEnumerable<Reservation>> GetReservationsByUserIdAsync(int userId);
    Task<IEnumerable<Reservation>> GetReservationsByCourtIdAsync(int courtId);
    Task<IEnumerable<Reservation>> GetReservationsByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Reservation>> GetConflictingReservationsAsync(int courtId, DateTime startTime, DateTime endTime, int? excludeReservationId = null);
    Task<Reservation> CreateReservationAsync(Reservation reservation);
    Task<Reservation> UpdateReservationAsync(int id, Reservation reservation);
    Task<Reservation> CancelReservationAsync(int id, string? cancellationReason);
    Task DeleteReservationAsync(int id);
}

