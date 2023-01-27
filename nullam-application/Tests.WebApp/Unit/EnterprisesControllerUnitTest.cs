using App.BLL.Services;
using App.Contracts.BLL;
using App.Contracts.DAL;
using App.DAL.EF;
using App.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using AutoMapper;
using WebApp.ApiControllers;
using Xunit.Abstractions;
using Enterprise = App.BLL.DTO.Enterprise;

namespace Tests.WebApp.Unit;

public class EnterprisesControllerUnitTest
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly AppDbContext _ctx;
    private readonly Mock<IEnterpriseRepository> _mockEnterpriseRepository;
    private readonly IMapper _DALBLLMapper;
    private readonly IMapper _DALDomainMapper;
    private readonly IMapper _BLLPublicMapper;
    private readonly EnterpriseService _enterpriseService;
    private readonly Mock<IAppBLL> _bllMock;
    private readonly EnterprisesController _enterprisesController;

    public EnterprisesControllerUnitTest(ITestOutputHelper testOutputHelper)
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
        
        _mockEnterpriseRepository = new Mock<IEnterpriseRepository>();
        
        // set up mock db - InMemory
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        _ctx = new AppDbContext(optionsBuilder.Options);

        _ctx.Database.EnsureDeleted();
        _ctx.Database.EnsureCreated();

        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

        _bllMock = new Mock<IAppBLL>();

        _mockEnterpriseRepository.Setup(x =>
            x.Add(It.Is<App.DAL.DTO.Enterprise>(e => e.GetType() == typeof(App.DAL.DTO.Enterprise)))).Returns(
            new App.DAL.DTO.Enterprise()
            {
                LegalName = "Elron OÜ",
                RegisterCode = "33445566",
                ParticipantsNumber = 2,
                AdditionalDetails = "Testing Add"
            });
        _mockEnterpriseRepository.Setup(x =>
            x.Update(It.Is<App.DAL.DTO.Enterprise>(e => e.LegalName == "Elron OÜ"))).Returns(
            new App.DAL.DTO.Enterprise()
            {
                LegalName = "Elron OÜ Updated",
                RegisterCode = "33445566",
                ParticipantsNumber = 2,
                AdditionalDetails = "Testing Updated"
            });        
        _mockEnterpriseRepository.Setup(x =>
            x.Remove(It.Is<Guid>(e => e == Guid.Empty))).Returns(
            new App.DAL.DTO.Enterprise()
            {
                LegalName = "Elron OÜ Remove",
                RegisterCode = "33445566",
                ParticipantsNumber = 2,
                AdditionalDetails = "Testing Remove"
            });
        _mockEnterpriseRepository.Setup(x => x.FirstOrDefaultAsync(
            It.Is<Guid>(a => a == Guid.Empty),
            It.Is<bool>(a => a == true))).ReturnsAsync(new App.DAL.DTO.Enterprise()
        {
            LegalName = "Elron OÜ FirstOrDefaultAsync",
            RegisterCode = "33445566",
            ParticipantsNumber = 2,
            AdditionalDetails = "Testing FirstOrDefaultAsync"
        });
        _mockEnterpriseRepository.Setup(x => x.GetAllAsync(true)).ReturnsAsync(new List<App.DAL.DTO.Enterprise>()
        {
            new()
            {
                LegalName = "Elron OÜ GetAllAsync1",
                RegisterCode = "33445566",
                ParticipantsNumber = 2,
                AdditionalDetails = "Testing GetAllAsync1"
            },            
            new()
            {
                LegalName = "Elron OÜ GetAllAsync2",
                RegisterCode = "33445566",
                ParticipantsNumber = 2,
                AdditionalDetails = "Testing GetAllAsync2"
            }
        }); 
        
        // custom methods
        _mockEnterpriseRepository.Setup(x =>
            x.GetEnterpriseByRegisterCode("33445566", Guid.Empty)).ReturnsAsync(new App.DAL.DTO.Enterprise()
        {
            LegalName = "Elron OÜ GetEnterpriseByRegisterCode",
            RegisterCode = "33445566",
            ParticipantsNumber = 2,
            AdditionalDetails = "Testing GetEnterpriseByRegisterCode"
        }); 
        
        
        _enterpriseService = new EnterpriseService(_mockEnterpriseRepository.Object, new App.BLL.Mappers.EnterpriseMapper(_DALBLLMapper));
        _enterprisesController = new EnterprisesController(_bllMock.Object, _BLLPublicMapper);
    }

    [Fact]
    public void Test_Enterprises_GetAllAsync()
    {
        var res = _enterpriseService.GetAllAsync(true).Result;

        List<App.BLL.DTO.Enterprise> listOfEnterprises = new()
        {
            new()
            {
                LegalName = "Elron OÜ GetAllAsync1",
                RegisterCode = "33445566",
                ParticipantsNumber = 2,
                AdditionalDetails = "Testing GetAllAsync1"
            },            
            new()
            {
                LegalName = "Elron OÜ GetAllAsync2",
                RegisterCode = "33445566",
                ParticipantsNumber = 2,
                AdditionalDetails = "Testing GetAllAsync2"
            }
        };

        var expected = listOfEnterprises.AsEnumerable();
        
        Assert.NotNull(res);
        Assert.NotStrictEqual(expected, res);
        
        _mockEnterpriseRepository.Verify(x => x.GetAllAsync(
            It.Is<bool>(a => a == true))
            , Times.Once);
    }

    [Fact]
    public void Test_Enterprises_FirstOrDefaultAsync()
    {
        var res = _enterpriseService.FirstOrDefaultAsync(Guid.Empty).Result;

        var enterpriseDto = new Enterprise()
        {
            LegalName = "Elron OÜ FirstOrDefaultAsync",
            RegisterCode = "33445566",
            ParticipantsNumber = 2,
            AdditionalDetails = "Testing FirstOrDefaultAsync"
        };

        var expected = enterpriseDto;
        
        Assert.NotNull(res);
        Assert.Equal(expected.LegalName, res.LegalName);
        Assert.Equal(expected.RegisterCode, res.RegisterCode);
        Assert.Equal(expected.ParticipantsNumber, res.ParticipantsNumber);
        Assert.Equal(expected.AdditionalDetails, res.AdditionalDetails);
        
        _mockEnterpriseRepository.Verify(x => x.FirstOrDefaultAsync(
            It.Is<Guid>(a => a.GetType() == typeof(Guid)),
            It.Is<bool>(a => a == true)
        ), Times.Once);
    }

    [Fact]
    public void Test_Enterprises_Add()
    {
        var res = _enterpriseService.Add(new Enterprise()
        {
            LegalName = "Elron OÜ",
            RegisterCode = "33445566",
            ParticipantsNumber = 2,
            AdditionalDetails = "Testing Add"
        });
        
        Assert.NotNull(res);
        Assert.True(res!.LegalName == "Elron OÜ");
        Assert.True(res!.RegisterCode == "33445566");
        Assert.True(res!.ParticipantsNumber == 2);
        Assert.True(res!.AdditionalDetails == "Testing Add");
    }

    [Fact]
    public void Test_Enterprises_Update()
    {
        var enterprise = new Enterprise()
        {
            LegalName = "Elron OÜ",
            RegisterCode = "33445566",
            ParticipantsNumber = 2,
            AdditionalDetails = "Testing Updated"
        };

        var res = _enterpriseService.Update(enterprise);

        Assert.NotNull(res);
        Assert.True(res!.LegalName == "Elron OÜ Updated");
        Assert.True(res!.RegisterCode == "33445566");
        Assert.True(res!.ParticipantsNumber == 2);
        Assert.True(res!.AdditionalDetails == "Testing Updated");
    }

    [Fact]
    public void Test_Enterprises_GetEnterpriseByRegisterCode()
    {
        var enterprise = new App.BLL.DTO.Enterprise()
        {
            LegalName = "Elron OÜ GetEnterpriseByRegisterCode",
            RegisterCode = "33445566",
            ParticipantsNumber = 2,
            AdditionalDetails = "Testing GetEnterpriseByRegisterCode"
        };
        
        var result = _enterpriseService.GetEnterpriseByRegisterCode(enterprise.RegisterCode, Guid.Empty).Result;
    
        var expected = enterprise;
        
        Assert.NotNull(result);
        Assert.Equal(expected.LegalName, result.LegalName);
        Assert.Equal(expected.RegisterCode, result.RegisterCode);
        Assert.Equal(expected.ParticipantsNumber, result.ParticipantsNumber);
        Assert.Equal(expected.AdditionalDetails, result.AdditionalDetails);
    }
    
    // [Fact]
    // public async void Test_Enterprises_GetEnterpriseByRegisterCode_Throw_Exception()
    // {
    //     var eventDto = new Event()
    //     {
    //         Id = Guid.NewGuid(),
    //         Name = "Testing",
    //         EventTime = DateTime.Now,
    //         Location = "Tallinn",
    //         AdditionalDetails = "Testing"
    //     };
    //     _ctx.Events.Add(eventDto);
    //     
    //     var enterprise = new App.BLL.DTO.Enterprise()
    //     {
    //         Id = Guid.NewGuid(),
    //         LegalName = "Elron OÜ GetEnterpriseByRegisterCode",
    //         RegisterCode = "33445567",
    //         ParticipantsNumber = 2,
    //         AdditionalDetails = "Testing GetEnterpriseByRegisterCode"
    //     };
    //     
    //     _ctx.Enterprises.Add(new App.Domain.Enterprise()
    //     {
    //         Id = enterprise.Id,
    //         LegalName = "Elron OÜ GetEnterpriseByRegisterCode",
    //         RegisterCode = "33445567",
    //         ParticipantsNumber = 2,
    //         AdditionalDetails = "Testing GetEnterpriseByRegisterCode"
    //     });
    //
    //     var paymentTypeDto = new PaymentType()
    //     {
    //         Id = Guid.NewGuid(),
    //         Name = "Sularaha",
    //         Comment = "Testing Sularaha"
    //     };
    //     _ctx.PaymentTypes.Add(paymentTypeDto);
    //     _ctx.Participants.Add(new Participant()
    //     {
    //         PaymentTypeId = paymentTypeDto.Id,
    //         EnterpriseId = enterprise.Id,
    //         EventId = eventDto.Id
    //     });
    //
    //     var result = _enterpriseService.GetEnterpriseByRegisterCode(enterprise.RegisterCode, enterprise.Id);
    //     
    //     // _testOutputHelper.WriteLine(result.RegisterCode);
    //     await Assert.ThrowsAnyAsync<Exception>(() =>
    //         _enterpriseService.GetEnterpriseByRegisterCode(enterprise.RegisterCode, Guid.Empty));
    // }
}