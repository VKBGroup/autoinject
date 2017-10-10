# AutoInject
[![license](https://img.shields.io/github/license/mashape/apistatus.svg)](https://github.com/VKBGroup/autoinject/blob/master/LICENSE) [![NuGet](https://img.shields.io/nuget/v/Nuget.Core.svg)](https://www.nuget.org/packages/VKB.AutoInject/) [![](https://img.shields.io/badge/.net%20core-2.0-AAC031.svg)]()

### What?
---
**AutoInject** is a very simple library that allows you to map your dependencies inside the concrete implementations.

### Why?
---
Hiding the depency mappings away in the Startup class or some other class makes for un-intuative usage. It's hard to know which service 
implementations are set for injection and what their scope will be without knowing where to look and what to look for. 
It also causes breaking changes if you switch a referenced project with only service implementations for a new one.

**AutoInject** makes it:
 - Easy to find mappings
 - Easy to see the scope of mappings
 - Easy to create new mappings
 - Easy to swap out implementations
 
 ### How?
---
 #### Step 1
 Grab the source and build it, download the latest binary release or get the package on NuGet.
 
 #### Step 2
 In your **Startup.cs** call the **AutoInject** extension on the ServiceCollection.
 ```csharp
public void ConfigureServices(IServiceCollection services)
{
  services.AddMvc();
  services.AutoInject();
}
 ```
 
 #### Step 3
 Add the **Inject** attribute to your service implementations along with the type of your service(to be injected) interface.
 The scope will default to *Transient* but can be overidden in the attribute constructor.
 ```csharp
[Inject(typeof(IYourService))]
class YourServiceImpl : IYourService
{
    ///...
}
```

You can also use more verbose attributes to cut out scoping manually as follows
```csharp
[InjectScoped(typeof(IYourOtherService))]
class YourOtherServiceImpl : IYourOtherService
{
    ///...
}
```

#### Step 4
You're done. Every new mapping now only requires an attribute as in **Step 3**.

Steps 1 to 3 would normally look like this, requiring looking up mappings in Startup.cs and registering each mapping here instead of in context.

**Startup.cs**
```csharp
public void ConfigureServices(IServiceCollection services)
{
  //...
  
  services.AddTransient<IYourService, IYourServiceImpl>();
  services.AddScoped<IYourOtherService, IYourOtherServiceImpl>();
  //... ad infinitum
}
 ```
 
 ### And?
---
 What about DbContext?
 
 #### Step 1
 In **Startup.cs** when calling the extension also pass a **IConfiguration** instance like so (if you want to use a connection name from appsettings.[profile].json or leave it parameterless if you want to specify the connection string.)
 ```csharp
public IConfiguration Configuration { get; }
public void ConfigureServices(IServiceCollection services)
{
  services.AddMvc();
  services.AutoInject(Configuration);
}
 ```
 
 #### Step 2
 In your class that extends **DbContext** add the following. The default scope will be *Scoped* but can be overriden in the constructor.
 
 **To Specify a connection string name**
 ```csharp
[EntityContext(name:"YourConnectionConfigName")]
public class YourEFDbContext : DbContext
{
}
 ```
 
  **To Specify a connection string**
 ```csharp
[EntityContext(connection: @"Server=YourSqlServer;Database=YourDBName;user=YourUser;password=y0urP4$$w0r6;")]
public class YourEFDbContext : DbContext
{
}
 ```
 
 **To specify neither**
 ```csharp
[EntityContext]
public class YourEFDbContext : DbContext
{
  protected override void OnConfiguring(DbContextOptionsBuilder options)
  {
    if (!options.IsConfigured)
    {
      options.UseSqlServer(@"Server=YourSqlServer;Database=YourDBName;user=YourUser;password=y0urP4$$w0r6;");
      //Or whatever mechanism you use
    }
  }
  // ...
}
 ```
 
 ### Done?
---
 Yes! You are all mapped up and ready to go.
 
 
