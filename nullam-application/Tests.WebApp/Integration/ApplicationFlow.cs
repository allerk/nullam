using System.Globalization;
using System.Text;
using App.Public.DTO.v1;
using Microsoft.AspNetCore.Mvc.Testing;
using Tests.WebApp.Helpers;
using Xunit.Abstractions;

namespace Tests.WebApp.Integration;

public class ApplicationFlow : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _testOutputHelper;

    public ApplicationFlow(CustomWebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        _factory = factory;
        _testOutputHelper = testOutputHelper;
        _client = _factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            }
        );
    }

    [Fact]
    public async void AddEvent()
    {
        // Arrange
        var uri = "api/v1.0/events/";

        var eventDto = new Event()
        {
            Name = "The Weeknd Tallinn 2023 Concert",
            EventTime = DateTime.Now.ToString(CultureInfo.InvariantCulture),
            Location = "Lauluväljak",
            AdditionalDetails = "This is the biggest upcoming Tallinn concert in 2023"
        };

        var eventToJson = JsonHelper.GetStringContent(eventDto);
        
        // Act
        var eventResponse = await _client.PostAsync(uri, eventToJson);
        
        // Assert
        eventResponse.EnsureSuccessStatusCode();
        
        Assert.Equal(201, (int) eventResponse.StatusCode);
        
        var body = await eventResponse.Content.ReadAsStringAsync();
        var data = JsonHelper.DeserializeWithWebDefaults<Event>(body);
        
        Assert.NotNull(data);
        Assert.Equal(eventDto.Name, data.Name);
        Assert.Equal(eventDto.EventTime, data.EventTime);
        Assert.Equal(eventDto.Location, data.Location);
        Assert.Equal(eventDto.AdditionalDetails, data.AdditionalDetails);
        _testOutputHelper.WriteLine("Event was successfully created!");
    }

    [Fact]
    public async void AddPersonParticipantToEvent()
    {
        // Arrange
        var uriParticipant = "api/v1.0/participants/";

        var eventDto = new Event()
        {
            Name = "The Weeknd Tallinn 2023 Concert",
            EventTime = DateTime.Now.ToString(CultureInfo.InvariantCulture),
            Location = "Lauluväljak",
            AdditionalDetails = "This is the biggest upcoming Tallinn concert in 2023"
        };

        var personDto = new Person()
        {
            FirstName = "Aleksandr",
            LastName = "Petrov",
            IdentificationNumber = "50008102225",
            AdditionalDetails = "TestingPerson"
        };

        var paymentTypeDto = new PaymentType()
        {
            Name = "Sularaha",
            Comment = "TestingPaymentType"
        };

        var participantDto = new Participant()
        {
            PaymentTypeId = paymentTypeDto.Id,
            PersonId = personDto.Id,
            EventId = eventDto.Id
        };

        var participantToJson = JsonHelper.GetStringContent(participantDto);

        // Act
        var participantResponse = await _client.PostAsync(uriParticipant, participantToJson);
        
        // Assert
        participantResponse.EnsureSuccessStatusCode();
        
        Assert.Equal(201, (int) participantResponse.StatusCode);
        
        var body = await participantResponse.Content.ReadAsStringAsync();
        var data = JsonHelper.DeserializeWithWebDefaults<Participant>(body);
        
        Assert.NotNull(data);
        Assert.Equal(participantDto.PaymentTypeId, data.PaymentTypeId);
        Assert.Equal(participantDto.PersonId, data.PersonId);
        Assert.Equal(participantDto.EventId, data.EventId);
        Assert.Equal(eventDto.Id, data.EventId);
        _testOutputHelper.WriteLine("Person participant was successfully added to event!");
    }
    
    [Fact]
    public async void AddEnterpriseParticipantToEvent()
    {
        // Arrange
        var uriParticipant = "api/v1.0/participants/";

        var eventDto = new Event()
        {
            Name = "Rammstein Tallinn 2022 Concert",
            EventTime = DateTime.Now.ToString(CultureInfo.InvariantCulture),
            Location = "Lauluväljak",
            AdditionalDetails = "This was the biggest Tallinn concert in 2022"
        };

        var enterpriseDto = new Enterprise()
        {
            LegalName = "Elron OÜ",
            RegisterCode = "4285647",
            ParticipantsNumber = 20,
            AdditionalDetails = "TestingEnterprise"
        };

        var paymentTypeDto = new PaymentType()
        {
            Name = "Swedbank",
            Comment = "TestingPaymentType"
        };

        var participantDto = new Participant()
        {
            PaymentTypeId = paymentTypeDto.Id,
            EnterpriseId = enterpriseDto.Id,
            EventId = eventDto.Id
        };

        var participantToJson = JsonHelper.GetStringContent(participantDto);

        // Act
        var participantResponse = await _client.PostAsync(uriParticipant, participantToJson);
        
        // Assert
        participantResponse.EnsureSuccessStatusCode();
        
        Assert.Equal(201, (int) participantResponse.StatusCode);
        
        var body = await participantResponse.Content.ReadAsStringAsync();
        var data = JsonHelper.DeserializeWithWebDefaults<Participant>(body);
        
        Assert.NotNull(data);
        Assert.Equal(participantDto.PaymentTypeId, data.PaymentTypeId);
        Assert.Equal(participantDto.EnterpriseId, data.EnterpriseId);
        Assert.Equal(participantDto.EventId, data.EventId);
        Assert.Equal(eventDto.Id, data.EventId);
        _testOutputHelper.WriteLine("Enterprise participant was successfully added to event!");
    }
}