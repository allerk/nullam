using App.BLL.DTO;
using App.BLL.Helpers;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class PersonService : BaseEntityService<Person, App.DAL.DTO.Person, IPersonRepository>, IPersonService
{
    public PersonService(IPersonRepository repository, IMapper<Person, DAL.DTO.Person> mapper) : base(repository, mapper)
    {
    }


    public new Person Add(Person person)
    {
        var idCodeValidator = new IDCodeValidation();
        if (idCodeValidator.IsValid(person.IdentificationNumber))
        {
            try
            {
                return Mapper.Map(Repository.Add(Mapper.Map(person)!))!;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        throw new Exception("Vale isikukood");
    }

    public new Person Update(Person person)
    {
        var idCodeValidator = new IDCodeValidation();
        if (idCodeValidator.IsValid(person.IdentificationNumber))
        {
            return Mapper.Map(Repository.Update(Mapper.Map(person)!))!;
        }
        
        throw new Exception("Vale isikukood");
    }

    public async Task<Person> GetPersonByIdentityCode(string identityCode, Guid eventId)
    {
        try
        {
            return Mapper.Map(await Repository.GetPersonByIdentityCode(identityCode, eventId))!;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}