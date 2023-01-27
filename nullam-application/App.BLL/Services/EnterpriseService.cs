using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class EnterpriseService : BaseEntityService<Enterprise, App.DAL.DTO.Enterprise, IEnterpriseRepository>, IEnterpriseService
{
    public EnterpriseService(IEnterpriseRepository repository, IMapper<Enterprise, DAL.DTO.Enterprise> mapper) : base(repository, mapper)
    {
    }

    public new Enterprise Add(Enterprise entity)
    {
        try
        {
            return Mapper.Map(Repository.Add(Mapper.Map(entity)!))!;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<Enterprise> GetEnterpriseByRegisterCode(string registerCode, Guid eventId)
    {
        try
        {
            return Mapper.Map(await Repository.GetEnterpriseByRegisterCode(registerCode, eventId))!;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}