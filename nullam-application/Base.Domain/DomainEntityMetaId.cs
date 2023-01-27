
using Base.Contracts.Domain;

namespace Base.Domain;

public abstract class DomainEntityMetaId : DomainEntityMetaId<Guid>, IDomainEntityId
{
    
}
public abstract class DomainEntityMetaId<TKey> : DomainEntityId<TKey>, IDomainEntityMeta
    where TKey : IEquatable<TKey>
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}