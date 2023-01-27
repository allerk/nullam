using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class PaymentTypeRepository : BaseEntityRepository<PaymentType, App.Domain.PaymentType, AppDbContext>, IPaymentTypeRepository
{
    public PaymentTypeRepository(AppDbContext dbContext, IMapper<PaymentType, Domain.PaymentType> mapper) : base(dbContext, mapper)
    {
    }
}