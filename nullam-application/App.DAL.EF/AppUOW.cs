using App.Contracts.DAL;
using App.DAL.EF.Mappers;
using App.DAL.EF.Repositories;
using Base.DAL.EF;

namespace App.DAL.EF;

public class AppUOW : BaseUOW<AppDbContext>, IAppUnitOfWork
{
    protected readonly AutoMapper.IMapper _mapper;
    public AppUOW(AppDbContext dbContext, AutoMapper.IMapper mapper) : base(dbContext)
    {
        _mapper = mapper;
    }

    private IEnterpriseRepository? _enterprises;
    public virtual IEnterpriseRepository Enterprises =>
        _enterprises ??= new EnterpriseRepository(UOWDbContext, new EnterpriseMapper(_mapper));    
    
    private IEventRepository? _events;
    public virtual IEventRepository Events =>
        _events ??= new EventRepository(UOWDbContext, new EventMapper(_mapper));    
    
    private IParticipantRepository? _participants;
    public virtual IParticipantRepository Participants =>
        _participants ??= new ParticipantRepository(UOWDbContext, new ParticipantMapper(_mapper));
    
    private IPaymentTypeRepository? _paymentTypes; 
    public virtual IPaymentTypeRepository PaymentTypes =>
        _paymentTypes ??= new PaymentTypeRepository(UOWDbContext, new PaymentTypeMapper(_mapper));    
    
    private IPersonRepository? _persons; 
    public virtual IPersonRepository Persons =>
        _persons ??= new PersonRepository(UOWDbContext, new PersonMapper(_mapper));

}