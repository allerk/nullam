using App.Contracts.BLL.Services;
using Base.Contracts.BLL;

namespace App.Contracts.BLL;

public interface IAppBLL : IBLL
{
    IEnterpriseService Enterprises { get; }
    IEventService Events { get; }
    IParticipantService Participants { get; }
    IPaymentTypeService PaymentTypes { get; }
    IPersonService Persons { get; }
}