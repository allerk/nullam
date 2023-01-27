using App.BLL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class PaymentTypeMapper : BaseMapper<PaymentType, App.DAL.DTO.PaymentType>
{
    public PaymentTypeMapper(IMapper mapper) : base(mapper)
    {
    }
}