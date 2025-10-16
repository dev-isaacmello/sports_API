namespace csharp_api.Application.Dtos.Court;

public class CreateCourtDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public decimal PricePerHour { get; set; }
    public bool IsCovered { get; set; }
    public int Capacity { get; set; }
}

