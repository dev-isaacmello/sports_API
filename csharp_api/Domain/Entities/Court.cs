namespace csharp_api.Domain.Entities;

public class Court
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // Futebol, Vôlei, Basquete, Tênis, etc
    public decimal PricePerHour { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsCovered { get; set; } = false; // Coberta ou ao ar livre
    public int Capacity { get; set; } // Capacidade máxima de pessoas
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navegação
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}

