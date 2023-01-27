using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class EventRepository : BaseEntityRepository<Event, App.Domain.Event, AppDbContext>, IEventRepository
{
    public EventRepository(AppDbContext dbContext, IMapper<Event, Domain.Event> mapper) : base(dbContext, mapper)
    {
    }

    public override async Task<IEnumerable<Event>> GetAllAsync(bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query
            .Include(e => e.Participants)
            .ThenInclude(x => x.Person)
            .Include(e => e.Participants)
            .ThenInclude(x => x.Enterprise)
            .Include(e => e.Participants)
            .ThenInclude(x => x.PaymentType);

        return (await query.ToListAsync()).Select(x => Mapper.Map(x)!);
    }

    public override async Task<Event?> FirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query
            .Include(e => e.Participants)
            .ThenInclude(x => x.Person)
            .Include(e => e.Participants)
            .ThenInclude(x => x.Enterprise)
            .Include(e => e.Participants)
            .ThenInclude(x => x.PaymentType);
        
        return Mapper.Map(await query.FirstOrDefaultAsync(x => x.Id == id));
    }
}