using MediaManager.Domain.Enums;

namespace MediaManager.Domain.Entities;

public class MediaItem
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public MediaType Tipo { get; private set; }
    public DateTime CriadoEm { get; private set; }
    public Guid UserId { get; private set; }

    public User User { get; private set; } = null!;
    public ICollection<ConsumptionRecord> ConsumptionRecords { get; private set; } = new List<ConsumptionRecord>();

    protected MediaItem() { }

    public MediaItem(string nome, MediaType tipo, Guid userId)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Tipo = tipo;
        UserId = userId;
        CriadoEm = DateTime.UtcNow;
    }

    public void Update(string nome)
    {
        Nome = nome;
    }
}
