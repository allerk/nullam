using App.Public.DTO.v1;
using AutoMapper;
using Base.DAL;

namespace App.Public.Mappers;

public class EnterpriseMapper : BaseMapper<Enterprise, App.BLL.DTO.Enterprise>
{
    public EnterpriseMapper(IMapper mapper) : base(mapper)
    {
    }
}