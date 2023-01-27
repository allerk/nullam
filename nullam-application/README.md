# Getting Started

## Tech stack:
- postgres:latest
- .NET Core 7
- as ide was used JetBrains Rider

### To be able to start:
1. use `docker compose up` create db
2. use `dotnet run` to run the application

If you are having troubles with nuget packages, use `nuget restore` to install all the packages. In my case I used Rider so didn't have this problem. 

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
