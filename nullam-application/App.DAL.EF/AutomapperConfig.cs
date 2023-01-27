using App.DAL.DTO;
using AutoMapper;

namespace App.DAL.EF;

public class AutomapperConfig : Profile
{
    public AutomapperConfig()
    {
        CreateMap<Enterprise, App.Domain.Enterprise>().ReverseMap();
        CreateMap<Event, App.Domain.Event>().ReverseMap();
        CreateMap<Participant, App.Domain.Participant>().ReverseMap();
        CreateMap<PaymentType, App.Domain.PaymentType>().ReverseMap();
        CreateMap<Person, App.Domain.Person>().ReverseMap();
    }
}