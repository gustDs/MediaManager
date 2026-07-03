using FluentValidation;

namespace MediaManager.Application.UseCases.Queries.ConsumptionRecord.GetAllConsumptionRecordsByMediaItem;

public class GetAllConsumptionRecordsByMediaItemValidator : AbstractValidator<GetAllConsumptionRecordsByMediaItemRequest>
{
    public GetAllConsumptionRecordsByMediaItemValidator()
    {
        RuleFor(x => x.MediaItemId)
            .NotEmpty().WithMessage("MediaItemId é obrigatório.")
            .Must(id => Guid.TryParse(id, out _)).WithMessage("MediaItemId deve ser um GUID válido.");
    }
}
