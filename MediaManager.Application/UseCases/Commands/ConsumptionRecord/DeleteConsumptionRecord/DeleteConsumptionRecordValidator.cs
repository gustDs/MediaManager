using FluentValidation;

namespace MediaManager.Application.UseCases.Commands.ConsumptionRecord.DeleteConsumptionRecord;

public class DeleteConsumptionRecordValidator : AbstractValidator<DeleteConsumptionRecordRequest>
{
    public DeleteConsumptionRecordValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id é obrigatório.")
            .Must(id => Guid.TryParse(id, out _)).WithMessage("Id deve ser um GUID válido.");
    }
}
