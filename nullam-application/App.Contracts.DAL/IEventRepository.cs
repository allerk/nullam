using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IEventRepository : IEntityRepository<Event>, IEventRepositoryCustom<Event>
{
    
}

public interface IEventRepositoryCustom<TEntity>
{
    // Task<TEntity> GetAllSortedByTimeAsync(bool noTracking = true);
}