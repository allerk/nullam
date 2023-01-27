using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class ParticipantMapper : BaseMapper<Participant, App.Domain.Participant>
{
    public ParticipantMapper(IMapper mapper) : base(mapper)
    {
    }
}