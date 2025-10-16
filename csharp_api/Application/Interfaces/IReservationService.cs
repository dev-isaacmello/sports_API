using csharp_api.Application.Dtos.Reservation;

namespace csharp_api.Application.Interfaces;

public interface IReservationService
{
    Task<IEnumerable<ReservationDto>> GetAllReservationsAsync();
    Task<ReservationDto> GetReservationByIdAsync(int id);
    Task<IEnumerable<ReservationDto>> GetReservationsByUserIdAsync(int userId);
    Task<IEnumerable<ReservationDto>> GetReservationsByCourtIdAsync(int courtId);
    Task<IEnumerable<ReservationDto>> GetReservationsByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<ReservationDto> CreateReservationAsync(int userId, CreateReservationDto createReservationDto);
    Task<ReservationDto> UpdateReservationAsync(int id, int userId, UpdateReservationDto updateReservationDto);
    Task<ReservationDto> CancelReservationAsync(int id, int userId, CancelReservationDto cancelReservationDto);
    Task DeleteReservationAsync(int id, int userId, bool isAdmin);
}

