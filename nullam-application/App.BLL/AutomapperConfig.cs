using App.BLL.DTO;
using AutoMapper;

namespace App.BLL;

public class AutomapperConfig : Profile
{
    public AutomapperConfig()
    {
        CreateMap<Enterprise, App.DAL.DTO.Enterprise>().ReverseMap();
        CreateMap<Event, App.DAL.DTO.Event>().ReverseMap();
        CreateMap<Participant, App.DAL.DTO.Participant>().ReverseMap();
        CreateMap<PaymentType, App.DAL.DTO.PaymentType>().ReverseMap();
        CreateMap<Person, App.DAL.DTO.Person>().ReverseMap();
    }
}