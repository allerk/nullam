using App.Public.DTO.v1;
using AutoMapper;
using Base.DAL;

namespace App.Public.Mappers;

public class PaymentTypeMapper : BaseMapper<PaymentType, App.BLL.DTO.PaymentType>
{
    public PaymentTypeMapper(IMapper mapper) : base(mapper)
    {
    }
}