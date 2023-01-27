# ApiControllers scaffolding
~~~sh
dotnet aspnet-codegenerator controller -name EnterprisesController -actions -m App.Domain.Enterprise -dc AppDbContext -outDir ApiControllers --useDefaultLayout -api --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name EventsController -actions -m App.Domain.Event -dc AppDbContext -outDir ApiControllers --useDefaultLayout -api --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name ParticipantsController -actions -m App.Domain.Participant -dc AppDbContext -outDir ApiControllers --useDefaultLayout -api --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name PaymentTypesController -actions -m App.Domain.PaymentType -dc AppDbContext -outDir ApiControllers --useDefaultLayout -api --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name PersonsController -actions -m App.Domain.Person -dc AppDbContext -outDir ApiControllers --useDefaultLayout -api --useAsyncActions --referenceScriptLibraries -f
~~~