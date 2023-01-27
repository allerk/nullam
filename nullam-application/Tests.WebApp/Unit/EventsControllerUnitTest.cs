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

public class EventsControllerUnitTest
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly AppDbContext _ctx;
    private readonly Mock<IEventRepository> _mockEventRepository;
    private readonly IMapper _DALBLLMapper;
    private readonly IMapper _DALDomainMapper;
    private readonly IMapper _BLLPublicMapper;
    private readonly EventService _eventService;
    private readonly Mock<IAppBLL> _bllMock;
    private readonly EventsController _eventsController;

    public EventsControllerUnitTest(ITestOutputHelper testOutputHelper)
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
        
        _mockEventRepository = new Mock<IEventRepository>();
        
        // set up mock db - InMemory
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        _ctx = new AppDbContext(optionsBuilder.Options);

        _ctx.Database.EnsureDeleted();
        _ctx.Database.EnsureCreated();

        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

        _bllMock = new Mock<IAppBLL>();

        _mockEventRepository.Setup(x =>
            x.Add(It.Is<App.DAL.DTO.Event>(e => e.GetType() == typeof(App.DAL.DTO.Event)))).Returns(
            new App.DAL.DTO.Event()
            {
                Name = "The Weeknd 2023",
                EventTime = DateTime.Parse("1/29/2023 2:05:00 PM"),
                Location = "Tallinn",
                AdditionalDetails = "Testing Add"
            });
        _mockEventRepository.Setup(x =>
            x.Update(It.Is<App.DAL.DTO.Event>(e => e.Name == "ConcertOld"))).Returns(
            new App.DAL.DTO.Event()
            {
                Name = "ConcertUpdated",
                EventTime = DateTime.Parse("1/29/2023 2:05:00 PM"),
                Location = "Tallinn",
                AdditionalDetails = "Testing Updated"
            });        
        _mockEventRepository.Setup(x =>
            x.Remove(It.Is<Guid>(e => e == Guid.Empty))).Returns(
            new App.DAL.DTO.Event()
            {
                Name = "ConcertRemove",
                EventTime = DateTime.Parse("1/29/2023 2:05:00 PM"),
                Location = "Tallinn",
                AdditionalDetails = "Testing Remove"
            });
        _mockEventRepository.Setup(x => x.FirstOrDefaultAsync(
            It.Is<Guid>(a => a == Guid.Empty),
            It.Is<bool>(a => a == true))).ReturnsAsync(new App.DAL.DTO.Event()
        {
            Name = "ConcertFirstOrDefaultAsync",
            EventTime = DateTime.Parse("1/29/2023 2:05:00 PM"),
            Location = "Tallinn",
            AdditionalDetails = "Testing FirstOfDefaultAsync"
        });
        _mockEventRepository.Setup(x => x.GetAllAsync(true)).ReturnsAsync(new List<App.DAL.DTO.Event>()
        {
            new()
            {
                Name = "ConcertGetAllAsync1",
                EventTime = DateTime.Parse("1/29/2023 2:05:00 PM"),
                Location = "Tallinn",
                AdditionalDetails = "Testing GetAllAsync1"
            },            
            new()
            {
                Name = "ConcertGetAllAsync2",
                EventTime = DateTime.Parse("1/29/2023 2:05:00 PM"),
                Location = "Tallinn",
                AdditionalDetails = "Testing GetAllAsync2"
            }
        }); 
        
        
        _eventService = new EventService(_mockEventRepository.Object, new App.BLL.Mappers.EventMapper(_DALBLLMapper));
        _eventsController = new EventsController(_bllMock.Object, _BLLPublicMapper);
    }

    [Fact]
    public void Test_Events_GetAllAsync()
    {
        var res = _eventService.GetAllAsync(true).Result;

        List<App.BLL.DTO.Event> listOfEvents = new()
        {
            new Event
            {
                Name = "ConcertGetAllAsync1",
                EventTime = DateTime.Parse("1/29/2023 2:05:00 PM"),
                Location = "Tallinn",
                AdditionalDetails = "Testing GetAllAsync1"
            },
            new Event
            {
                Name = "ConcertGetAllAsync2",
                EventTime = DateTime.Parse("1/29/2023 2:05:00 PM"),
                Location = "Tallinn",
                AdditionalDetails = "Testing GetAllAsync2"
            }
        };

        var expected = listOfEvents.AsEnumerable();
        
        Assert.NotNull(res);
        Assert.NotStrictEqual(expected, res);
        
        _mockEventRepository.Verify(x => x.GetAllAsync(
            It.Is<bool>(a => a == true))
            , Times.Once);
    }

    [Fact]
    public void Test_Events_FirstOrDefaultAsync()
    {
        var res = _eventService.FirstOrDefaultAsync(Guid.Empty).Result;

        var eventDto = new Event()
        {
            Name = "ConcertFirstOrDefaultAsync",
            EventTime = DateTime.Parse("1/29/2023 2:05:00 PM"),
            Location = "Tallinn",
            AdditionalDetails = "Testing FirstOfDefaultAsync"
        };

        var expected = eventDto;
        
        Assert.NotNull(res);
        Assert.Equal(expected.Name, res.Name);
        Assert.Equal(expected.EventTime, res.EventTime);
        Assert.Equal(expected.Location, res.Location);
        Assert.Equal(expected.AdditionalDetails, res.AdditionalDetails);
        
        _mockEventRepository.Verify(x => x.FirstOrDefaultAsync(
            It.Is<Guid>(a => a.GetType() == typeof(Guid)),
            It.Is<bool>(a => a == true)
        ), Times.Once);
    }

    [Fact]
    public void Test_Events_Add()
    {
        var res = _eventService.Add(new Event()
        {
            Name = "The Weeknd 2023",
            EventTime = DateTime.Parse("1/29/2023 2:05:00 PM"),
            Location = "Tallinn",
            AdditionalDetails = "Testing Add"
        });
        
        Assert.NotNull(res);
        Assert.True(res!.Name == "The Weeknd 2023");
        Assert.True(res!.EventTime == DateTime.Parse("1/29/2023 2:05:00 PM"));
        Assert.True(res!.Location == "Tallinn");
        Assert.True(res!.AdditionalDetails == "Testing Add");
    }

    [Fact]
    public void Test_Events_Update()
    {
        var eventDto = new Event()
        {
            Name = "ConcertOld",
            EventTime = DateTime.Parse("1/29/2023 2:05:00 PM"),
            Location = "Tallinn",
            AdditionalDetails = "Testing Old"
        };

        var res = _eventService.Update(eventDto);
        
        Assert.NotNull(res);
        Assert.True(res!.Name == "ConcertUpdated");
        Assert.True(res!.EventTime == DateTime.Parse("1/29/2023 2:05:00 PM"));
        Assert.True(res!.Location == "Tallinn");
        Assert.True(res!.AdditionalDetails == "Testing Updated");
    }

    [Fact]
    public void Test_Events_Remove()
    {
        var eventRemove = new App.BLL.DTO.Event()
        {
            Name = "ConcertRemove",
            EventTime = DateTime.Parse("1/29/2023 2:05:00 PM"),
            Location = "Tallinn",
            AdditionalDetails = "Testing Remove"
        };
        
        var result = _eventService.Remove(eventRemove.Id);
    
        var expected = eventRemove;
            
        Assert.NotNull(result);
        Assert.Equal(expected.Name, result.Name);
        Assert.Equal(expected.EventTime, result.EventTime);
        Assert.Equal(expected.Location, result.Location);
        Assert.Equal(expected.AdditionalDetails, result.AdditionalDetails);
            
        _mockEventRepository.Verify(x => x.Remove(
            It.Is<Guid>(a => a == Guid.Empty)), Times.Once);
    }
    
}