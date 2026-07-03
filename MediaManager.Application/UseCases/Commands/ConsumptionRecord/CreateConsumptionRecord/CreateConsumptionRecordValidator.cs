using FluentValidation;
using MediaManager.Domain.Enums;

namespace MediaManager.Application.UseCases.Commands.ConsumptionRecord.CreateConsumptionRecord;

public class CreateConsumptionRecordValidator : AbstractValidator<CreateConsumptionRecordRequest>
{
    public CreateConsumptionRecordValidator()
    {
        RuleFor(x => x.MediaItemId)
            .NotEmpty().WithMessage("MediaItemId é obrigatório.")
            .Must(id => Guid.TryParse(id, out _)).WithMessage("MediaItemId deve ser um GUID válido.");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status é obrigatório.")
            .Must(s => Enum.TryParse<ConsumptionStatus>(s, ignoreCase: true, out _))
            .WithMessage("Status inválido. Use: Do, Doing ou Done.");

        When(x => x.Nota.HasValue, () =>
        {
            RuleFor(x => x.Nota!.Value)
                .InclusiveBetween(0.5m, 5.0m).WithMessage("Nota deve estar entre 0.5 e 5.0.")
                .Must(n => n % 0.5m == 0).WithMessage("Nota deve ser múltiplo de 0.5.");
        });
    }
}
