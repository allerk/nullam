using App.BLL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class EnterpriseMapper : BaseMapper<Enterprise, App.DAL.DTO.Enterprise>
{
    public EnterpriseMapper(IMapper mapper) : base(mapper)
    {
    }
}