using FluentValidation;

namespace MediaManager.Application.UseCases.Queries.Media.GetMediaById;

public class GetMediaByIdValidator : AbstractValidator<GetMediaByIdRequest>
{
    public GetMediaByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id é obrigatório.")
            .Must(id => Guid.TryParse(id, out _)).WithMessage("Id deve ser um GUID válido.");
    }
}
