using csharp_api.Application.Dtos.Reservation;
using FluentValidation;

namespace csharp_api.Application.Validation;

public class CreateReservationDtoValidator : AbstractValidator<CreateReservationDto>
{
    public CreateReservationDtoValidator()
    {
        RuleFor(x => x.CourtId)
            .GreaterThan(0).WithMessage("ID da quadra inválido");

        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("Horário de início é obrigatório")
            .GreaterThan(DateTime.UtcNow).WithMessage("Horário de início deve ser no futuro");

        RuleFor(x => x.EndTime)
            .NotEmpty().WithMessage("Horário de término é obrigatório")
            .GreaterThan(x => x.StartTime).WithMessage("Horário de término deve ser após o horário de início");

        RuleFor(x => x.Notes)
            .MaximumLength(500).WithMessage("Observações devem ter no máximo 500 caracteres");

        RuleFor(x => x)
            .Must(x => (x.EndTime - x.StartTime).TotalHours >= 1)
            .WithMessage("Duração mínima da reserva é de 1 hora")
            .Must(x => (x.EndTime - x.StartTime).TotalHours <= 8)
            .WithMessage("Duração máxima da reserva é de 8 horas");
    }
}

