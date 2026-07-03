namespace MediaManager.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public DateTime CriadoEm { get; private set; }

    public ICollection<MediaItem> MediaItems { get; private set; } = new List<MediaItem>();

    protected User() { }

    public User(string email, string passwordHash)
    {
        Id = Guid.NewGuid();
        Email = email;
        PasswordHash = passwordHash;
        CriadoEm = DateTime.UtcNow;
    }
}
