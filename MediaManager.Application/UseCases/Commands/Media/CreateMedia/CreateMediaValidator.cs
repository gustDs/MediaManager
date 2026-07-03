using FluentValidation;
using MediaManager.Domain.Enums;

namespace MediaManager.Application.UseCases.Commands.Media.CreateMedia;

public class CreateMediaValidator : AbstractValidator<CreateMediaRequest>
{
    public CreateMediaValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MaximumLength(500).WithMessage("Nome deve ter no máximo 500 caracteres.");

        RuleFor(x => x.Tipo)
            .NotEmpty().WithMessage("Tipo é obrigatório.")
            .Must(t => Enum.TryParse<MediaType>(t, ignoreCase: true, out _))
            .WithMessage("Tipo inválido. Use: Filme, Serie, Jogo ou Livro.");
    }
}
