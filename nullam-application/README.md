# Getting Started

## Tech stack:
- postgres:latest
- .NET Core 7
- as IDE was used JetBrains Rider

### To be able to start:
1. use `docker compose up` create db
2. use `dotnet run` to run the application

If you are having troubles with nuget packages, use `nuget restore` to install all the packages. In my case I used Rider so didn't have this problem. 

### Migrations F.A.Q
- [Commands link 1](nullam/nullam-application/App.DAL.EF/README.md)
- [Commands link 2](https://github.com/allerk/nullam/blob/main/nullam-application/App.DAL.EF/README.md)

But you can configure auto migration, drop db in /WebApp/appsettings.json

### User manual
1. User (there is no identity system) can create an event and manage it. It's forbidden for him to create an event in a past.
2. User can create participant (enterprise or person) and added it to event. PaymentTypes are set initially, when app was started. To be able to add new, you can add them manually via e.g Postman. Person and Enterprise have different properties, so that in some cases validation might be different. They can't be deleted, only participant. It was did to use one person/enterprise in multiple events.
3. User can change their details (not participant).
4. User can delete participants from an event (but not person/enterprise).
5. Person/Enterprise live forever from the moment they were made. They can be only changed. There is no opportunity to add another person/enterprise with same id/register code on client side. To controll it, there is speciall methods in related repos/services.
6. User can delete only upcoming events. Every crud operation except read is unable for held events. The same is for participants that are in held event.
7. All the error messages, if something goes wrong, appear immediately.

### Architecture

Basically layers are: Domain -> DAL -> BLL -> Public endpoints.

#### Domain
- Basic database entities. And that's it!
#### DAL (Data Access Layer)
- DbContext
- Migrations
- Appropriate DTO's, Mappers, etc..
- Repositories
- UOW (Unit of Work)
#### BLL (Buisness logic layer)
- Services
- Application logic
- Appropriate DTO's, Mappers, etc..
#### Public endpoints
- Object that will be send to client side
- Versioning
- Appropriate DTO's, Mappers, etc..

Every layer (except Domain and Public) has their own features (e.g like their interfaces or configuration files).

Base services and repositories has all the methods with generic type to be able to use them in different controllers. For every base method exists appropriate interfaces. It's possible to write you own method and/or override existing to change logic or the way how data is processed (it was used several times here).

#### WebApp
Contains all the ApiControllers and startup configurations. For data initialization, or some fucntions authomatization exists AppDataHelper (configuration in appsettings.json `DataInitialization`). Program.cs contains startup configurations, dependency injections, builder options etc..

#### ERD Schema
![alt text](https://github.com/allerk/nullam/blob/main/nullam-application/erd.png?raw=true)

Event has one to many relation with Participant, as well as Person and Enterprise have one to many with Participant and PaymentType also. Basically Participant is a in-between-table between Event and Enterprise/Person.

# My thoughts:
To be honest, I know that it is better to replace some of the logic from the front-end to the back-end, like partial validation or some forbiddens to manage data seed. But as was earlier decided sending project at Friday. Basically all necessary requirements are done.
