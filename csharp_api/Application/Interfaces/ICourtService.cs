using csharp_api.Application.Dtos.Court;

namespace csharp_api.Application.Interfaces;

public interface ICourtService
{
    Task<IEnumerable<CourtDto>> GetAllCourtsAsync();
    Task<IEnumerable<CourtDto>> GetActiveCourtsAsync();
    Task<CourtDto> GetCourtByIdAsync(int id);
    Task<IEnumerable<CourtDto>> GetCourtsByTypeAsync(string type);
    Task<CourtDto> CreateCourtAsync(CreateCourtDto createCourtDto);
    Task<CourtDto> UpdateCourtAsync(int id, UpdateCourtDto updateCourtDto);
    Task DeleteCourtAsync(int id);
}

