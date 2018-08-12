# README

These projects create some nuget packages that form the backbone of my own applications. They are not
intended for public use, however you are free to do so.

Be aware that versioning does not follow semantic versioning, and that all the nuget packages produced
by these projects are "together" in the sense that version ranges are not considered. If you upgrade/use
one of them, you *probably* need to upgrade them all. As I said, they're not intended for anyone but me.

# To create a new ASP.NET Core MVC application using DryIoc

1. Create the new application
2. Add the following nuget packages:
   * JetBrains.Annotations
   * DryIoc.dll
   * LVK.AppCore.Mvc
3. Go into Startup.cs and modify ConfigureServices to this:

       public IServiceProvider ConfigureServices(IServiceCollection services)
       {
           services.AddMvc().AddControllersAsServices();
 
           return MvcAppBootstrapper.Bootstrap<ServicesBootstrapper>(services);
       }
4. Add a new class for ServicesBootstrapper, implementing IServicesBootstrapper

   You can leave it empty for now if you don't have any services
    
That's it