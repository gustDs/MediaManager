using MediaManager.Domain.Enums;

namespace MediaManager.Domain.Entities;

public class MediaItem
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public MediaType Tipo { get; private set; }
    public DateTime CriadoEm { get; private set; }

    public ICollection<ConsumptionRecord> ConsumptionRecords { get; private set; } = new List<ConsumptionRecord>();

    protected MediaItem() { }

    public MediaItem(string nome, MediaType tipo)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Tipo = tipo;
        CriadoEm = DateTime.UtcNow;
    }

    public void Update(string nome)
    {
        Nome = nome;
    }
}
