using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class EnterpriseRepository : BaseEntityRepository<Enterprise, App.Domain.Enterprise, AppDbContext>, IEnterpriseRepository
{
    public EnterpriseRepository(AppDbContext dbContext, IMapper<Enterprise, Domain.Enterprise> mapper) : base(dbContext, mapper)
    {
    }

    public override Enterprise Add(Enterprise enterprise)
    {
        if (RepoDbSet.Any(x => x.RegisterCode == enterprise.RegisterCode))
        {
            throw new Exception("Selle registreerimiskoodiga ettevõtte on juba olemas");
        }
        return Mapper.Map(RepoDbSet.Add(Mapper.Map(enterprise)!).Entity)!;
    }

    public async Task<Enterprise> GetEnterpriseByRegisterCode(string registerCode, Guid eventId)
    {
        var query = CreateQuery();
        var currentEvent = RepoDbContext.Events
            .Include(e => e.Participants)
            .ThenInclude(e => e.Enterprise)
            .FirstOrDefault(x => x.Id == eventId);
        query = query
            .Include(p => p.Participant);

        if (currentEvent!.Participants != null)
        {
            foreach (var participant in currentEvent.Participants)
            {
                if (participant.Enterprise != null)
                {
                    if (participant.Enterprise!.RegisterCode == registerCode)
                    {
                        throw new Exception("Selle registreerimiskoodiga ettevõtte on juba olemas süsteemis");
                    }   
                }
            }
        }
        
        var res = await query.FirstOrDefaultAsync(x => x.RegisterCode == registerCode);

        return Mapper.Map(res)!;
    }
}