using AutoMapper;
using csharp_api.Application.Dtos.Court;
using csharp_api.Application.Interfaces;
using csharp_api.Domain.Entities;
using csharp_api.Domain.Interfaces;

namespace csharp_api.Application.Services;

public class CourtService(ICourtRepository courtRepository, IMapper mapper) : ICourtService
{
    public async Task<IEnumerable<CourtDto>> GetAllCourtsAsync()
    {
        var courts = await courtRepository.GetAllCourtsAsync();
        return mapper.Map<IEnumerable<CourtDto>>(courts);
    }

    public async Task<IEnumerable<CourtDto>> GetActiveCourtsAsync()
    {
        var courts = await courtRepository.GetActiveCourtsAsync();
        return mapper.Map<IEnumerable<CourtDto>>(courts);
    }

    public async Task<CourtDto> GetCourtByIdAsync(int id)
    {
        var court = await courtRepository.GetCourtByIdAsync(id)
            ?? throw new KeyNotFoundException($"Quadra com ID {id} não encontrada");
        
        return mapper.Map<CourtDto>(court);
    }

    public async Task<IEnumerable<CourtDto>> GetCourtsByTypeAsync(string type)
    {
        if (string.IsNullOrWhiteSpace(type))
        {
            throw new ArgumentException("Tipo de quadra não pode ser vazio", nameof(type));
        }

        var courts = await courtRepository.GetCourtsByTypeAsync(type);
        return mapper.Map<IEnumerable<CourtDto>>(courts);
    }

    public async Task<CourtDto> CreateCourtAsync(CreateCourtDto createCourtDto)
    {
        // Validações de negócio
        if (createCourtDto.PricePerHour <= 0)
        {
            throw new InvalidOperationException("Preço por hora deve ser maior que zero");
        }

        if (createCourtDto.Capacity <= 0)
        {
            throw new InvalidOperationException("Capacidade deve ser maior que zero");
        }

        var court = mapper.Map<Court>(createCourtDto);
        court.CreatedAt = DateTime.UtcNow;
        court.IsActive = true;

        var createdCourt = await courtRepository.CreateCourtAsync(court);
        return mapper.Map<CourtDto>(createdCourt);
    }

    public async Task<CourtDto> UpdateCourtAsync(int id, UpdateCourtDto updateCourtDto)
    {
        var existingCourt = await courtRepository.GetCourtByIdAsync(id)
            ?? throw new KeyNotFoundException($"Quadra com ID {id} não encontrada");

        // Validações de negócio
        if (updateCourtDto.PricePerHour.HasValue && updateCourtDto.PricePerHour.Value <= 0)
        {
            throw new InvalidOperationException("Preço por hora deve ser maior que zero");
        }

        if (updateCourtDto.Capacity.HasValue && updateCourtDto.Capacity.Value <= 0)
        {
            throw new InvalidOperationException("Capacidade deve ser maior que zero");
        }

        // Aplicar apenas os campos que foram enviados
        if (!string.IsNullOrWhiteSpace(updateCourtDto.Name))
            existingCourt.Name = updateCourtDto.Name;
        
        if (!string.IsNullOrWhiteSpace(updateCourtDto.Description))
            existingCourt.Description = updateCourtDto.Description;
        
        if (!string.IsNullOrWhiteSpace(updateCourtDto.Type))
            existingCourt.Type = updateCourtDto.Type;
        
        if (updateCourtDto.PricePerHour.HasValue)
            existingCourt.PricePerHour = updateCourtDto.PricePerHour.Value;
        
        if (updateCourtDto.IsActive.HasValue)
            existingCourt.IsActive = updateCourtDto.IsActive.Value;
        
        if (updateCourtDto.IsCovered.HasValue)
            existingCourt.IsCovered = updateCourtDto.IsCovered.Value;
        
        if (updateCourtDto.Capacity.HasValue)
            existingCourt.Capacity = updateCourtDto.Capacity.Value;

        var updatedCourt = await courtRepository.UpdateCourtAsync(id, existingCourt);
        return mapper.Map<CourtDto>(updatedCourt);
    }

    public async Task DeleteCourtAsync(int id)
    {
        var court = await courtRepository.GetCourtByIdAsync(id)
            ?? throw new KeyNotFoundException($"Quadra com ID {id} não encontrada");

        await courtRepository.DeleteCourtAsync(id);
    }
}

