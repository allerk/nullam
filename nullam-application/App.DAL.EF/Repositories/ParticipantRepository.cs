using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class ParticipantRepository : BaseEntityRepository<Participant, App.Domain.Participant, AppDbContext>, IParticipantRepository
{
    public ParticipantRepository(AppDbContext dbContext, IMapper<Participant, Domain.Participant> mapper) : base(dbContext, mapper)
    {
    }

    public override async Task<IEnumerable<Participant>> GetAllAsync(bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query
            .Include(e => e.PaymentType)
            .Include(e => e.Person)
            .Include(e => e.Enterprise);

        return (await query.ToListAsync()).Select(x => Mapper.Map(x)!);
    }

    public override async Task<Participant?> FirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query
            .Include(e => e.PaymentType)
            .Include(e => e.Person)
            .Include(e => e.Enterprise);
        
        return Mapper.Map(await query.FirstOrDefaultAsync(x => x.Id == id));
    }
}