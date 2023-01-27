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

public class ParticipantsControllerUnitTest
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly AppDbContext _ctx;
    private readonly Mock<IParticipantRepository> _mockParticipantRepository;
    private readonly IMapper _DALBLLMapper;
    private readonly IMapper _DALDomainMapper;
    private readonly IMapper _BLLPublicMapper;
    private readonly ParticipantService _participantService;
    private readonly Mock<IAppBLL> _bllMock;
    private readonly ParticipantsController _participantsController;
    
    public ParticipantsControllerUnitTest(ITestOutputHelper testOutputHelper)
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
        
        _mockParticipantRepository = new Mock<IParticipantRepository>();
        
        // set up mock db - InMemory
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        _ctx = new AppDbContext(optionsBuilder.Options);

        _ctx.Database.EnsureDeleted();
        _ctx.Database.EnsureCreated();

        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

        _bllMock = new Mock<IAppBLL>();

        _mockParticipantRepository.Setup(x =>
            x.Add(It.Is<App.DAL.DTO.Participant>(e => e.GetType() == typeof(App.DAL.DTO.Participant)))).Returns(
            new App.DAL.DTO.Participant()
            {
                PaymentTypeId = Guid.NewGuid(),
                PersonId = Guid.NewGuid(),
                EventId = Guid.NewGuid()
            });
        _mockParticipantRepository.Setup(x =>
            x.Update(It.Is<App.DAL.DTO.Participant>(e => e.EventId == Guid.Empty))).Returns(
            new App.DAL.DTO.Participant()
            {
                PaymentTypeId = Guid.NewGuid(),
                PersonId = Guid.NewGuid(),
                EventId = Guid.NewGuid()
            });        
        _mockParticipantRepository.Setup(x =>
            x.Remove(It.Is<Guid>(e => e == Guid.Empty))).Returns(
            new App.DAL.DTO.Participant()
            {
                PaymentTypeId = Guid.NewGuid(),
                PersonId = Guid.NewGuid(),
                EventId = Guid.NewGuid()
            });
        _mockParticipantRepository.Setup(x => x.FirstOrDefaultAsync(
            It.Is<Guid>(a => a == Guid.Empty),
            It.Is<bool>(a => a == true))).ReturnsAsync(new App.DAL.DTO.Participant()
        {
            PaymentTypeId = Guid.NewGuid(),
            PersonId = Guid.NewGuid(),
            EventId = Guid.NewGuid()
        });
        _mockParticipantRepository.Setup(x => x.GetAllAsync(true)).ReturnsAsync(new List<App.DAL.DTO.Participant>()
        {
            new()
            {
                PaymentTypeId = Guid.NewGuid(),
                PersonId = Guid.NewGuid(),
                EventId = Guid.NewGuid()
            },            
            new()
            {
                PaymentTypeId = Guid.NewGuid(),
                PersonId = Guid.NewGuid(),
                EventId = Guid.NewGuid()
            }
        });


        _participantService = new ParticipantService(_mockParticipantRepository.Object, new App.BLL.Mappers.ParticipantMapper(_DALBLLMapper));
        _participantsController = new ParticipantsController(_bllMock.Object, _BLLPublicMapper);
    }
    
    [Fact]
    public void Test_Participants_GetAllAsync()
    {
        var res = _participantService.GetAllAsync(true).Result;

        List<App.BLL.DTO.Participant> listOfEnterprises = new()
        {
            new()
            {
                PaymentTypeId = Guid.NewGuid(),
                PersonId = Guid.NewGuid(),
                EventId = Guid.NewGuid()
            },            
            new()
            {
                PaymentTypeId = Guid.NewGuid(),
                PersonId = Guid.NewGuid(),
                EventId = Guid.NewGuid()
            }
        };

        var expected = listOfEnterprises.AsEnumerable();
        
        Assert.NotNull(res);
        Assert.NotStrictEqual(expected, res);
        
        _mockParticipantRepository.Verify(x => x.GetAllAsync(
                It.Is<bool>(a => a == true))
            , Times.Once);
    }
}