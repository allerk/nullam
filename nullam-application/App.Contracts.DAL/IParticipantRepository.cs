using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IParticipantRepository : IEntityRepository<Participant>, IParticipantRepositoryCustom<Participant>
{
    
}

public interface IParticipantRepositoryCustom<TEntity>
{
    
}