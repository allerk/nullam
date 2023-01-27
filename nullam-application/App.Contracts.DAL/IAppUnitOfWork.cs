using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IAppUnitOfWork : IUnitOfWork
{
    public IEnterpriseRepository Enterprises { get; }
    public IEventRepository Events { get; }
    public IParticipantRepository Participants { get; }
    public IPaymentTypeRepository PaymentTypes { get; }
    public IPersonRepository Persons { get; }
}