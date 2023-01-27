using App.Public.DTO.v1;
using AutoMapper;
using Base.DAL;

namespace App.Public.Mappers;

public class EventMapper : BaseMapper<Event, App.BLL.DTO.Event>
{
    public EventMapper(IMapper mapper) : base(mapper)
    {
    }
}