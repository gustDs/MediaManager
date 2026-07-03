using FluentValidation;

namespace MediaManager.Application.UseCases.Commands.Media.DeleteMedia;

public class DeleteMediaValidator : AbstractValidator<DeleteMediaRequest>
{
    public DeleteMediaValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id é obrigatório.")
            .Must(id => Guid.TryParse(id, out _)).WithMessage("Id deve ser um GUID válido.");
    }
}
