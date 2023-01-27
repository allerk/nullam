using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IPaymentTypeRepository : IEntityRepository<PaymentType>, IPaymentTypeRepositoryCustom<PaymentType>
{
    
}

public interface IPaymentTypeRepositoryCustom<TEntity>
{
    
}