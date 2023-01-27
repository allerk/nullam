using App.Public.DTO.v1;
using AutoMapper;

namespace App.Public;

public class AutomapperConfig : Profile
{
    public AutomapperConfig()
    {
        CreateMap<Enterprise, App.BLL.DTO.Enterprise>().ReverseMap();
        CreateMap<Event, App.BLL.DTO.Event>().ReverseMap();
        CreateMap<Participant, App.BLL.DTO.Participant>().ReverseMap();
        CreateMap<PaymentType, App.BLL.DTO.PaymentType>().ReverseMap();
        CreateMap<Person, App.BLL.DTO.Person>().ReverseMap();
    }
}