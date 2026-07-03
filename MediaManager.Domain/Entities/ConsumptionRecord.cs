using MediaManager.Domain.Enums;

namespace MediaManager.Domain.Entities;

public class ConsumptionRecord
{
    public Guid Id { get; private set; }
    public Guid MediaItemId { get; private set; }
    public ConsumptionStatus Status { get; private set; }
    public DateTime? DataInicio { get; private set; }
    public DateTime? DataFim { get; private set; }
    public decimal? Nota { get; private set; }
    public string? Resenha { get; private set; }
    public int? HorasJogadas { get; private set; }
    public int? PaginasLidas { get; private set; }
    public DateTime CriadoEm { get; private set; }

    public MediaItem MediaItem { get; private set; } = null!;

    protected ConsumptionRecord() { }

    public ConsumptionRecord(
        Guid mediaItemId,
        ConsumptionStatus status,
        DateTime? dataInicio = null,
        decimal? nota = null,
        string? resenha = null,
        int? horasJogadas = null,
        int? paginasLidas = null)
    {
        Id = Guid.NewGuid();
        MediaItemId = mediaItemId;
        Status = status;
        DataInicio = dataInicio;
        Nota = nota;
        Resenha = resenha;
        HorasJogadas = horasJogadas;
        PaginasLidas = paginasLidas;
        CriadoEm = DateTime.UtcNow;
    }

    public void Update(
        ConsumptionStatus status,
        DateTime? dataInicio,
        DateTime? dataFim,
        decimal? nota,
        string? resenha,
        int? horasJogadas,
        int? paginasLidas)
    {
        Status = status;
        DataInicio = dataInicio;
        DataFim = dataFim;
        Nota = nota;
        Resenha = resenha;
        HorasJogadas = horasJogadas;
        PaginasLidas = paginasLidas;
    }
}
