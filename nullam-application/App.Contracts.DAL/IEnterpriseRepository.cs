using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IEnterpriseRepository : IEntityRepository<Enterprise>, IEnterpriseRepositoryCustom<Enterprise>
{
    
}

public interface IEnterpriseRepositoryCustom<TEntity>
{
    Task<TEntity> GetEnterpriseByRegisterCode(string registerCode, Guid eventId);
}