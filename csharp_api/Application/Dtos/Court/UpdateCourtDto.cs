namespace csharp_api.Application.Dtos.Court;

public class UpdateCourtDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Type { get; set; }
    public decimal? PricePerHour { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsCovered { get; set; }
    public int? Capacity { get; set; }
}

