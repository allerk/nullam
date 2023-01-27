using System.Collections;
using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class EventService : BaseEntityService<Event, App.DAL.DTO.Event, IEventRepository>, IEventService
{
    public EventService(IEventRepository repository, IMapper<Event, DAL.DTO.Event> mapper) : base(repository, mapper)
    {
    }


    public new async Task<IEnumerable<Event>> GetAllAsync(bool noTracking = true)
    {
        var unsortedList = (await Repository.GetAllAsync()).Select(x => Mapper.Map(x)!).ToList();
        var res = new List<Event>();

        foreach (var item in unsortedList)
        {
            if (item.EventTime > DateTime.Now)
            {
                res.Add(item);
            }
            else
            {
                item.IsHeld = true;
                res.Add(item);
            }
        }
        return res;
    }

    public new async Task<Event?> FirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        var res = Mapper.Map(await Repository.FirstOrDefaultAsync(id, noTracking));

        if (res != null && res.EventTime < DateTime.Now)
        {
            res.IsHeld = true;
        }

        return res;
    }
}