using csharp_api.Application.Dtos.Court;
using FluentValidation;

namespace csharp_api.Application.Validation;

public class CreateCourtDtoValidator : AbstractValidator<CreateCourtDto>
{
    public CreateCourtDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .MinimumLength(3).WithMessage("Nome deve ter pelo menos 3 caracteres")
            .MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Descrição deve ter no máximo 500 caracteres");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Tipo é obrigatório")
            .MaximumLength(50).WithMessage("Tipo deve ter no máximo 50 caracteres");

        RuleFor(x => x.PricePerHour)
            .GreaterThan(0).WithMessage("Preço por hora deve ser maior que zero")
            .LessThanOrEqualTo(1000).WithMessage("Preço por hora deve ser no máximo R$ 1.000,00");

        RuleFor(x => x.Capacity)
            .GreaterThan(0).WithMessage("Capacidade deve ser maior que zero")
            .LessThanOrEqualTo(100).WithMessage("Capacidade deve ser no máximo 100 pessoas");
    }
}

