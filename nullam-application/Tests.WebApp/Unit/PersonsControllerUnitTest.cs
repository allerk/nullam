using App.BLL.DTO;
using App.BLL.Services;
using App.Contracts.BLL;
using App.Contracts.DAL;
using App.DAL.EF;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using WebApp.ApiControllers;
using Xunit.Abstractions;

namespace Tests.WebApp.Unit;

public class PersonsControllerUnitTest
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly AppDbContext _ctx;
    private readonly Mock<IPersonRepository> _mockPersonRepository;
    private readonly IMapper _DALBLLMapper;
    private readonly IMapper _DALDomainMapper;
    private readonly IMapper _BLLPublicMapper;
    private readonly PersonService _personService;
    private readonly Mock<IAppBLL> _bllMock;
    private readonly PersonsController _personsController;
    
    public PersonsControllerUnitTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        
        var profileBll = new App.BLL.AutomapperConfig();
        var configurationBLL = new MapperConfiguration(cfg => cfg.AddProfile(profileBll));
        IMapper mapperBLL = new Mapper(configurationBLL);
        
        var profileDAL = new App.DAL.EF.AutomapperConfig();
        var configurationDAL = new MapperConfiguration(cfg => cfg.AddProfile(profileDAL));
        
        _DALDomainMapper = new Mapper(configurationDAL);

        _DALBLLMapper = mapperBLL;

        _BLLPublicMapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new App.Public.AutomapperConfig())));
        
        _mockPersonRepository = new Mock<IPersonRepository>();
        
        // set up mock db - InMemory
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        _ctx = new AppDbContext(optionsBuilder.Options);

        _ctx.Database.EnsureDeleted();
        _ctx.Database.EnsureCreated();

        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

        _bllMock = new Mock<IAppBLL>();

        _mockPersonRepository.Setup(x =>
            x.Add(It.Is<App.DAL.DTO.Person>(e => e.GetType() == typeof(App.DAL.DTO.Person)))).Returns(
            new App.DAL.DTO.Person()
            {
                FirstName = "Aleksandr",
                LastName = "Pertrov",
                IdentificationNumber = "50007102225",
                AdditionalDetails = "Testing Add"
            });
        _mockPersonRepository.Setup(x =>
            x.Update(It.Is<App.DAL.DTO.Person>(e => e.IdentificationNumber == "50007102225"))).Returns(
            new App.DAL.DTO.Person()
            {
                FirstName = "Aleksandr Updated",
                LastName = "Pertrov",
                IdentificationNumber = "50007102225",
                AdditionalDetails = "Testing Updated"
            });        
        _mockPersonRepository.Setup(x =>
            x.Remove(It.Is<Guid>(e => e == Guid.Empty))).Returns(
            new App.DAL.DTO.Person()
            {
                FirstName = "Aleksandr Removed",
                LastName = "Pertrov",
                IdentificationNumber = "50007102225",
                AdditionalDetails = "Testing Removed"
            });
        _mockPersonRepository.Setup(x => x.FirstOrDefaultAsync(
            It.Is<Guid>(a => a == Guid.Empty),
            It.Is<bool>(a => a == true))).ReturnsAsync(new App.DAL.DTO.Person()
        {
            FirstName = "Aleksandr FirstOrDefaultAsync",
            LastName = "Pertrov",
            IdentificationNumber = "50007102225",
            AdditionalDetails = "Testing FirstOrDefaultAsync"
        });
        _mockPersonRepository.Setup(x => x.GetAllAsync(true)).ReturnsAsync(new List<App.DAL.DTO.Person>()
        {
            new()
            {
                FirstName = "Aleksandr GetAllAsync1",
                LastName = "Kovtonjuk",
                IdentificationNumber = "50007102225",
                AdditionalDetails = "Testing GetAllAsync1"
            },            
            new()
            {
                FirstName = "Aleksandr GetAllAsync2",
                LastName = "Kovtonjuk",
                IdentificationNumber = "50007102225",
                AdditionalDetails = "Testing GetAllAsync2"
            }
        });

        _personService = new PersonService(_mockPersonRepository.Object, new App.BLL.Mappers.PersonMapper(_DALBLLMapper));
        _personsController = new PersonsController(_bllMock.Object, _BLLPublicMapper);
    }
    
    [Fact]
    public void Test_Persons_GetAllAsync()
    {
        var res = _personService.GetAllAsync(true).Result;

        List<App.BLL.DTO.Person> listOfEnterprises = new()
        {
            new()
            {
                FirstName = "Aleksandr GetAllAsync1",
                LastName = "Kovtonjuk",
                IdentificationNumber = "50007102225",
                AdditionalDetails = "Testing GetAllAsync1"
            },            
            new()
            {
                FirstName = "Aleksandr GetAllAsync2",
                LastName = "Kovtonjuk",
                IdentificationNumber = "50007102225",
                AdditionalDetails = "Testing GetAllAsync2"
            }
        };

        var expected = listOfEnterprises.AsEnumerable();
        
        Assert.NotNull(res);
        Assert.NotStrictEqual(expected, res);
        
        _mockPersonRepository.Verify(x => x.GetAllAsync(
                It.Is<bool>(a => a == true))
            , Times.Once);
    }
    
    [Fact]
    public void Test_Persons_FirstOrDefaultAsync()
    {
        var res = _personService.FirstOrDefaultAsync(Guid.Empty).Result;

        var personDto = new Person()
        {
            FirstName = "Aleksandr FirstOrDefaultAsync",
            LastName = "Pertrov",
            IdentificationNumber = "50007102225",
            AdditionalDetails = "Testing FirstOrDefaultAsync"
        };

        var expected = personDto;
        
        Assert.NotNull(res);
        Assert.Equal(expected.FirstName, res.FirstName);
        Assert.Equal(expected.LastName, res.LastName);
        Assert.Equal(expected.IdentificationNumber, res.IdentificationNumber);
        Assert.Equal(expected.AdditionalDetails, res.AdditionalDetails);
        
        _mockPersonRepository.Verify(x => x.FirstOrDefaultAsync(
            It.Is<Guid>(a => a.GetType() == typeof(Guid)),
            It.Is<bool>(a => a == true)
        ), Times.Once);
    }
    
    [Fact]
    public void Test_Persons_Add()
    {
        var res = _personService.Add(new Person()
        {
            FirstName = "Aleksandr",
            LastName = "Pertrov",
            IdentificationNumber = "50007102225",
            AdditionalDetails = "Testing Add"
        });
        
        Assert.NotNull(res);
        Assert.True(res!.FirstName == "Aleksandr");
        Assert.True(res!.LastName == "Pertrov");
        Assert.True(res!.IdentificationNumber == "50007102225");
        Assert.True(res!.AdditionalDetails == "Testing Add");
    }
    
    [Fact]
    public void Test_Persons_Add_Should_Return_Exception()
    {
        var ex = Record.Exception(() => _personService.Add(new Person()
        {
            FirstName = "Aleksandr",
            LastName = "Pertrov",
            IdentificationNumber = "50008102225",
            AdditionalDetails = "Testing Add"
        }));
        
        Assert.IsType<Exception>(ex);
        Assert.Equal("Vale isikukood", ex.Message);
    }
    
    [Fact]
    public void Test_Persons_Update()
    {
        var person = new Person()
        {
            FirstName = "Aleksandr Updated",
            LastName = "Pertrov",
            IdentificationNumber = "50007102225",
            AdditionalDetails = "Testing Updated"
        };

        var res = _personService.Update(person);

        Assert.NotNull(res);
        Assert.True(res!.FirstName == "Aleksandr Updated");
        Assert.True(res!.LastName == "Pertrov");
        Assert.True(res!.IdentificationNumber == "50007102225");
        Assert.True(res!.AdditionalDetails == "Testing Updated");
    }
}