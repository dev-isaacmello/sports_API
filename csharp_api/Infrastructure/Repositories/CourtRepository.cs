using csharp_api.Domain.Entities;
using csharp_api.Domain.Interfaces;
using csharp_api.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace csharp_api.Infrastructure.Repositories;

public class CourtRepository(AppDbContext context) : ICourtRepository
{
    public async Task<IEnumerable<Court>> GetAllCourtsAsync()
    {
        return await context.Courts.ToListAsync();
    }

    public async Task<IEnumerable<Court>> GetActiveCourtsAsync()
    {
        return await context.Courts.Where(c => c.IsActive).ToListAsync();
    }

    public async Task<Court?> GetCourtByIdAsync(int id)
    {
        return await context.Courts.FindAsync(id);
    }

    public async Task<IEnumerable<Court>> GetCourtsByTypeAsync(string type)
    {
        return await context.Courts
            .Where(c => c.Type.ToLower() == type.ToLower() && c.IsActive)
            .ToListAsync();
    }

    public async Task<Court> CreateCourtAsync(Court court)
    {
        await context.Courts.AddAsync(court);
        await context.SaveChangesAsync();
        return court;
    }

    public async Task<Court> UpdateCourtAsync(int id, Court court)
    {
        var existingCourt = await context.Courts.FindAsync(id)
            ?? throw new KeyNotFoundException($"Quadra com ID {id} não encontrada");
        
        court.Id = id;
        context.Entry(existingCourt).CurrentValues.SetValues(court);
        await context.SaveChangesAsync();
        return existingCourt;
    }

    public async Task DeleteCourtAsync(int id)
    {
        var court = await context.Courts.FindAsync(id)
            ?? throw new KeyNotFoundException($"Quadra com ID {id} não encontrada");
        
        context.Courts.Remove(court);
        await context.SaveChangesAsync();
    }
}

