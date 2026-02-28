namespace Engine.Persistance.Models
{
    public sealed record LeaseInfo(string OwnerId, DateTimeOffset AcquiredUtc, DateTimeOffset ExpiresUtc);
}
