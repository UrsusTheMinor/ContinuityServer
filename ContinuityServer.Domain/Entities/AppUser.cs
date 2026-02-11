namespace ContinuityServer.Domain.Entities;

public sealed class AppUser
{
    public Guid Id { get; set; }
    public string AuthSubject { get; set; } = ""; // Auth0 "sub"
    public string DisplayName { get; set; } = "";
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
