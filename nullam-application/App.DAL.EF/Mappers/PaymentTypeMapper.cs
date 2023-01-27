using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class PaymentTypeMapper : BaseMapper<PaymentType, App.Domain.PaymentType>
{
    public PaymentTypeMapper(IMapper mapper) : base(mapper)
    {
    }
}