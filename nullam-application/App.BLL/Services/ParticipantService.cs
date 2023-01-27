using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class ParticipantService : BaseEntityService<Participant, App.DAL.DTO.Participant, IParticipantRepository>, IParticipantService
{
    public ParticipantService(IParticipantRepository repository, IMapper<Participant, DAL.DTO.Participant> mapper) : base(repository, mapper)
    {
    }
}