# DAL related material
## EF commands:
To be able to create, delete or modify migrations:
~~~sh
dotnet ef migrations add --project App.DAL.EF --startup-project WebApp --context AppDbContext Initial
dotnet ef database update --project App.DAL.EF --startup-project WebApp --context AppDbContext
dotnet ef database remove --project App.DAL.EF --startup-project WebApp
~~~
To  drop db:
~~~sh
dotnet ef database drop --project App.DAL.EF --startup-project WebApp
~~~
