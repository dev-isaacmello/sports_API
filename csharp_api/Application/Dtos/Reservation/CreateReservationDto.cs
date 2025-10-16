namespace csharp_api.Application.Dtos.Reservation;

public class CreateReservationDto
{
    public int CourtId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? Notes { get; set; }
}

