using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class PersonRepository : BaseEntityRepository<Person, App.Domain.Person, AppDbContext>, IPersonRepository
{
    public PersonRepository(AppDbContext dbContext, IMapper<Person, Domain.Person> mapper) : base(dbContext, mapper)
    {
    }

    public override Person Add(Person person)
    {
        if (RepoDbSet.Any(x => x.IdentificationNumber == person.IdentificationNumber))
        {
            throw new Exception("Selle isikukoodiga kasutaja on juba olemas");
        }
        return Mapper.Map(RepoDbSet.Add(Mapper.Map(person)!).Entity)!;
    }

    public async Task<Person> GetPersonByIdentityCode(string identityCode, Guid eventId)
    {
        var query = CreateQuery();
        var currentEvent = RepoDbContext.Events
            .Include(e => e.Participants)
            .ThenInclude(p => p.Person)
            .FirstOrDefault(x => x.Id == eventId);
        query = query
            .Include(p => p.Participant);

        if (currentEvent!.Participants != null)
        {
            foreach (var participant in currentEvent.Participants)
            {
                if (participant.Person != null)
                {
                    if (participant.Person!.IdentificationNumber == identityCode)
                    {
                        throw new Exception("Selle isikukoodiga kasutaja on juba olemas süsteemis");
                    }   
                }
            }
        }
        
        var res = await query.FirstOrDefaultAsync(x => x.IdentificationNumber == identityCode);

        return Mapper.Map(res)!;
    }
}