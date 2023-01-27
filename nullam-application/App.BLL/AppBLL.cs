using App.BLL.Mappers;
using App.BLL.Services;
using App.Contracts.BLL;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using AutoMapper;
using Base.BLL;

namespace App.BLL;

public class AppBLL: BaseBll<IAppUnitOfWork>, IAppBLL
{
    protected IAppUnitOfWork UnitOfWork;
    protected readonly AutoMapper.IMapper _mapper;
    public AppBLL(IAppUnitOfWork unitOfWork, IMapper mapper)
    {
        UnitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public override async Task<int> SaveChangesAsync()
    {
        return await UnitOfWork.SaveChangesAsync();
    }

    public override int SaveChanges()
    {
        return UnitOfWork.SaveChanges();
    }

    private IEnterpriseService? _enterprises;
    public IEnterpriseService Enterprises =>
        _enterprises ??= new EnterpriseService(UnitOfWork.Enterprises, new EnterpriseMapper(_mapper));    
    
    private IEventService? _events;
    public IEventService Events =>
        _events ??= new EventService(UnitOfWork.Events, new EventMapper(_mapper));    
    
    private IParticipantService? _participants;
    public IParticipantService Participants =>
        _participants ??= new ParticipantService(UnitOfWork.Participants, new ParticipantMapper(_mapper));

    private IPaymentTypeService? _paymentTypes;
    public IPaymentTypeService PaymentTypes =>
        _paymentTypes ??= new PaymentTypeService(UnitOfWork.PaymentTypes, new PaymentTypeMapper(_mapper));    
    
    private IPersonService? _persons;
    public IPersonService Persons =>
        _persons ??= new PersonService(UnitOfWork.Persons, new PersonMapper(_mapper));
}