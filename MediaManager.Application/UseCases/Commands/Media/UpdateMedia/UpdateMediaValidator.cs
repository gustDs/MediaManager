using FluentValidation;

namespace MediaManager.Application.UseCases.Commands.Media.UpdateMedia;

public class UpdateMediaValidator : AbstractValidator<UpdateMediaRequest>
{
    public UpdateMediaValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id é obrigatório.")
            .Must(id => Guid.TryParse(id, out _)).WithMessage("Id deve ser um GUID válido.");

        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MaximumLength(500).WithMessage("Nome deve ter no máximo 500 caracteres.");
    }
}
