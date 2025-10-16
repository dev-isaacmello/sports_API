using csharp_api.Domain.Entities;

namespace csharp_api.Domain.Interfaces;

public interface ICourtRepository
{
    Task<IEnumerable<Court>> GetAllCourtsAsync();
    Task<IEnumerable<Court>> GetActiveCourtsAsync();
    Task<Court?> GetCourtByIdAsync(int id);
    Task<IEnumerable<Court>> GetCourtsByTypeAsync(string type);
    Task<Court> CreateCourtAsync(Court court);
    Task<Court> UpdateCourtAsync(int id, Court court);
    Task DeleteCourtAsync(int id);
}

