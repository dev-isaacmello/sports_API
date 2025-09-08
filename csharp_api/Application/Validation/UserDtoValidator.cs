using csharp_api.Application.Dtos;
using FluentValidation;

namespace csharp_api.Application.Validation;

public class UserDtoValidator : AbstractValidator<UserDto>
{
    public UserDtoValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().WithMessage("O nome é obrigatório.").MaximumLength(30).WithMessage("O nome deve ter no máximo 30 caracteres.");
        RuleFor(x => x.Telefone).NotEmpty().WithMessage("O telefone é obrigatório.").MaximumLength(15).WithMessage("O telefone deve ter no máximo 15 caracteres.");
    }
}