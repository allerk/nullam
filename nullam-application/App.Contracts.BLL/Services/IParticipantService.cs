using App.BLL.DTO;
using App.Contracts.DAL;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IParticipantService : IEntityService<Participant>, IParticipantRepositoryCustom<Participant>
{
    
}