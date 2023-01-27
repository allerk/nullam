using App.Public.DTO.v1;
using AutoMapper;
using Base.DAL;

namespace App.Public.Mappers;

public class ParticipantMapper : BaseMapper<Participant, App.BLL.DTO.Participant>
{
    public ParticipantMapper(IMapper mapper) : base(mapper)
    {
    }
}