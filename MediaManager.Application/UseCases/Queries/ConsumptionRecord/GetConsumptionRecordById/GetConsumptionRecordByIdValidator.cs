using FluentValidation;

namespace MediaManager.Application.UseCases.Queries.ConsumptionRecord.GetConsumptionRecordById;

public class GetConsumptionRecordByIdValidator : AbstractValidator<GetConsumptionRecordByIdRequest>
{
    public GetConsumptionRecordByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id é obrigatório.")
            .Must(id => Guid.TryParse(id, out _)).WithMessage("Id deve ser um GUID válido.");
    }
}
