using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class EnterpriseMapper : BaseMapper<Enterprise, App.Domain.Enterprise>
{
    public EnterpriseMapper(IMapper mapper) : base(mapper)
    {
    }
}