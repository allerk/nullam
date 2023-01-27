using App.BLL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class ParticipantMapper : BaseMapper<Participant, App.DAL.DTO.Participant>
{
    public ParticipantMapper(IMapper mapper) : base(mapper)
    {
    }
}