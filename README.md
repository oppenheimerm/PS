# Building ASP.NET Core Web APIs with Clean Architecture

## How Clean Architecture works

The key rule behind Clean Architecture is: The **Dependency Rule**. The gist of this is simply that *dependencies are encapsulated in each "ring" of the architecture model and these dependencies can only point inward*.

![Clean Architecture Ring](../GitImages/clean-architecture-circle-diagram.jpg)




Clean Architecture keeps details like web frameworks and databases in the outer layers while important business rules and policies are housed in the inner circles and have no knowledge of anything outside of themselves.

Considering this, you can start to see how it achieves a very clean separation of concerns. By ensuring that your **business rules and core domain logic in the inner circles are completely devoid of any external dependencies, or 3rd party libraries** means they **must be expressed using pure C# POCO classes** which makes testing them much easier.

>In fact, your business rules simply don’t know anything at all about the outside world. - Robert C. Martin



##  Elements of Clean Architecture

####    Entities

Entities are the heart of clean architecture and contain any *enterprise-wide* business rules and logic.  If you're writing a standalone application Uncle Bob suggests simply referring to these as Business Objects. The key is that they contain rules that are not application specific - so basically **any global or shareable logic that could be reused in other applications should be encapsulated in an entity**.

####    Use Cases

Moving up from the entities we have the **Use Case layer**. The classes that live here have a few unique features and responsibilities:

-   Contain the *application specific* business rules.
-   Encapsulate and implement all of the use cases of the system. A good rule to start with is a class per use case.
-   Orchestrate the flow of data to and from the entities, and can rely on their business rules to achieve the goals of the use case
-   **Have NO dependency and are totally isolated from things like a database, UI or special frameworks**
-   Will almost certainly require refactoring if details of the use case requirements change.

Use case classes are *typically suffixed with the word Interactor*. Uncle Bob mentions in this talk that he considered calling them controllers but assumed this would be too easily confused with MVC so Interactor it is!

####    Interface Adapters

The purpose of the interface adapter layer is to act as a connector between the business logic in our interactors and our framework-specific code. For example, in an *ASP.Net MVC app, this is where the models, views, and controllers live*. Gateways like *services and repositories are also implemented here*.


####    Presentation


####    Frameworks and Drivers
This layer contains tools like databases or frameworks. By default, we don’t write very much code in this layer, but it’s important to clearly state the place and priority that those tools have in the architecture.


---
##  Demo Application

Here is the project structure representing each of the logical layers:

![alt text](../notes/images/aspnet-core-web-api-project-structure.png)

We can further elaborate on these layers:

**Web.Api** - Maps to the layers that hold the **Web**, **UI** and **Presenter** concerns. In the context of our API, this means it accepts input in the form of http requests over the network (e.g., GET/POST/etc.) and returns its output as content formatted as JSON/HTML/XML, etc. The Presenters contain .NET framework-specific references and types, so they also live here as per The Dependency Rule we don't want to pass any of them inward.

**Web.Api.Core** - Maps to the layers that hold the **Use Case and Entity concerns and is also where our External Interfaces get defined**. These innermost layers contain our domain objects and business rules. The **code in this layer is mostly pure C# - no network connections, databases, etc. allowed**. Interfaces represent those dependencies, and their implementations get injected into our use cases.

**Web.Api.Infrastructure** - Maps to the layers that hold the Database and Gateway concerns. Here we define *data entities, database access (typically in the shape of repositories), integrations with other network services, caches, etc*. This project/layer contains the physical implementation of the interfaces defined in our domain layer.

we now have a skeletal solution spun up that looks to support the high-level layers requested by Clean Architecture - cool! 😎


##  Using Tests to Guide Development (TDD)

With our project foundation in place, we're ready to start writing some interesting code. To start, we're going to *build out our use cases "from the outside in" by defining them first and then using tests to iteratively tease out and implement only the bits of functionality required to pass them* - red-green-refactor - this is the essence of TDD.

Our API needs to provide the capability for new users to create an account from the client application so our first use case will be RegisterUserUseCase.

I quickly fleshed out a test containing the things I think must happen to satisfy the use case. As you can see below, there's lots of red here at the moment, and this won't even build, but it's **a terrific guide that tells us precisely what we must implement thereby driving our development and ensuring we only work on the bits required to get this from red to green**:


![alt text](../notes/images/tdd-clean-architecture-failing-red-use-case-test.png)


In `RegisterUserUseCase` initially has a single dependency on `UserRepository` which we know will have the responsibility of persisting the user's identity. We're not sure exactly how that will happen at this point and that's fine, we'll come back to that shortly.

To implement `RegisterUserUseCase` : **Web.Api.Core.RegisterUserUseCase** we first created a simple interface named `IRegisterUserUseCase` : **Web.Api.Core.Interfaces.UseCases.IRegisterUserUseCase** which in turn implements `IUseCaseRequestHandler` : **Web.Api.Core.Interfaces.IUseCaseRequestHandler** that will define the shape of all of our use case classes.


Note on naming; you may see the term *Interactor* being used in place of Use Case or even combined, i.e., *Use Case Interactor* in other Clean Architecture implementations. As far as I can tell they serve the same purpose and *both represent the classes where the business logic lives*. I prefer Use Case as I find it's a little more intuitive.

Next, I took at a crack and implemented the absolute bare minimum amount of code required by the use case at this point. My first cut looked like this:

![alt text](../notes/images/tdd-clean-architecture-partially-completed-use-case.png)

TDD is an iterative process (red->-green->refactor...repeat) and it will take a few iterations to refine this gradually.


####    Input and Output Ports

You'll notice the signature for the use case's `Handle()` method contains two parameters: **`message`** and **`outputPort`**. The message parameter is an Input Port whose sole responsibility is to carry the request model into the use case from the upper layer that triggers it (UI, controller etc.). This class is simply a lightweight DTO owned by the Core/Domain layer.

The second, **`outputPort`** is responsible for getting data out of the use case into a form suitable for its caller. The critical difference is that it needs to be an interface with at least one method available for our **Presenter** to implement. The physical implementation of the presenter lives in the outer layer and may contain UI/View/Framework specifics and dependencies that we don't want bleeding into our use cases.

In this approach, we use [DIP](https://en.wikipedia.org/wiki/Dependency_inversion_principle) to invert the control flow, so rather than getting a return value with the response data from the use case we allow it to decide when and where it sends the data through the output port to the presenter. We could use a callback, but for our purposes, as you'll see shortly, our presenter creates an http response message that is returned by our controller. *In the context of a REST API, this response is effectively the "UI"* or at very least the output data to be displayed by whatever application might be consuming it. The question of whether or not the use case should call the presenter is a good one, and the answer is likely whichever is most viable for your solution.

The **IOutputPort** contract is pretty simple; it exposes a single method accepting a generic use case response model it will transform into the final format required by the outer layer. We'll see an example implementation shortly.

Next, I created a contract representing **`IUserRepository`** with a single method for creating new users:

````
public interface IUserRepository
{
  Task<CreateUserResponse> Create(User user, string password);       
}
````

####    From Red to Green
With a few more iterations the interfaces are complete, and the use case has no more red squiqqles. I moved back to the unit test and touched it up by creating mocks for both collaborators using the suitably named [Moq](https://www.nuget.org/packages/moq/)  mocking library.

Things are looking much better - the project compiles, and we can run the test and it passes! We have a thin slice of functionality working to register new users.

##  The Data Layer

Most programs require data to make them somewhat interesting or valuable, and our API is no different. Moving up a layer we'll find the data layer. This layer contains our data access and persistence concerns and any frameworks, drivers or dependencies they require.

In this layer, we'll make decisions, and lay down concrete implementations of the various data providers our application requires. Persistence and data access are handled using Sql Server and Entity Framework Core. All data access related dependencies and code live in `Web.Api.Infrastructure`.


We need to provision the infrastructure project with the following nuget packages:
- `Microsoft.EntityFrameworkCore`
  -`Microsoft.EntityFrameworkCore.Design`
- `Microsoft.EntityFrameworkCore.SqlServer` 

along with code for the UserRepository and a data entity to represent our User.

The implementation of `UserRepository` at this point is pretty simple. It depends on `UserManager` which we get out of the box from the identity provider and `IMapper` which comes from [AutoMapper](https://www.nuget.org/packages/automapper/)  and enables us to elegantly map domain, data and dto objects across the layers of our application while saving us from littering our project with repetitive boilerplate code. `UserManager` provides APIs for managing users in the membership database.

In our `Create()` we start off by mapping our domain user to the data entity `AppUser` and then call the `CreateAsync()`on `_userManager` to create the user in the database and returning the result.

All of the work to validate the input parameters and create the user is handled by `UserManager` and everything is abstracted behind the `IUserRepository` interface. In this sense, it is acting mostly as a **facade**(See notes),
 to keep these implementation details encapsulated in the Infrastructure layer.

### Using Migrations to Spin Up a Database

We're ready to use Entity Framework's migrations to create the physical sql database.  First we need to make some changes to the startup project, in this case `Web.Api`:

we need to add the following packages to our startup project:
    **Microsoft.EntityFrameworkCore.SqlServer**
    **Microsoft.AspNetCore.Identity.UI**

Update public void ConfigureServices(IServiceCollection services)(in `Startup.cs`)

````
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<ApplicationDbContext>(options => {
                options.UseSqlServer(Configuration["ConnectionStrings:AppApiConnectionString"]);
            });

            services.AddDefaultIdentity<AppUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            ...
            ...
            ...
        }

````

Add our connection strin to the `appsettings.json` file:

````
 "ConnectionStrings": {
    "AppApiConnectionString": "Server=(localdb)\\mssqllocaldb;Database=CleanCodeDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  },

````


Add `ApplicationDbContext.cs`(our db context class) in directory: Web.Api.Infrastructure/Data/EntityFramework/

````
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Infrastructure.Data.Entities;

namespace Web.Api.Infrastructure.Data.EntityFramework
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {}

        public override int SaveChanges()
        {
            AddAuitInfo();
            return base.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            AddAuitInfo();
            return await base.SaveChangesAsync();
        }

        private void AddAuitInfo()
        {
            var entries = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    ((BaseEntity)entry.Entity).Created = DateTime.UtcNow;
                }
                ((BaseEntity)entry.Entity).Modified = DateTime.UtcNow;
            }
        }

    }
}

````


####    Helpful EF command line commands: 

**You need to be in the Web.Api.Infrastructure directory for the below to work**

>  list - show available dbcontext:
````
    dotnet ef dbcontext list
````

> info - info about our dbContext
````
    dotnet ef dbcontext info -s ..\Web.Api\Web.Api
````
** where "-s" points to startup project **

And the below moves us backup one directory into the web directory, which
        contains our **Startup.cs** - `ConfigureServices() `

Create migrations for our database.  Again, we **need to be in the directory of our dbcontext which is our Web.Api.Infrastructure** directory.

````
dotnet ef migrations add initialcreate -s ..\Web.ApiWeb.Api

````

>  Update database

````
dotnet ef database update -s ..\Web.Api\Web.Api.csproj

````

In the directory of our dbcontext which is our Web.Api.Infrastructure** directory.

---

##  Dependency Injection

##  The presentation layer

This is where the different components across our architecture are composted into a single, cohesive unit - **The Application**.

Here is where we find concerns related to *GUIs*, *web pages*, *devices*, etc. It also contains our **Presenters** which, as mentioned, are responsible for formatting the response data from our use cases into a convenient format for delivery to whatever human interface our user happens to trigger it from, e.g., a web page, mobile app, microwave, etc.

In the context of a **REST API**, this layer is relatively simple as there is no GUI, view state or complex user interactions to handle. The request/response semantics of REST means we're mostly just delivering data back to the user. *Our presenters, in this case, will construct http messages containing the data and status of the request and return these as the response from our Web API controller** operations.

RegisterUserPresenter(`Web.Api.RegisterUserPresenter.cs`) implements `IOutportPort` with all of its work happening in the `Handle()`. You'll remember **this is the method the use case triggers**. From there, we're just building a regular *MVC ContentResult* and in this case, directly storing the serialized results from the use case and setting the appropriate HttpStatusCode based on the result of the use case.

##  Setting Up the Controller

API controller represents the very furthest edge of our Clean Architecture. The controller will receive input in the form of http requests, trigger our use case and respond with some meaningful message based on the outcome.

##  Notes

1.  Better Software Design with Clean Architecture - https://fullstackmark.com/post/11/better-software-design-with-clean-architecture  

2.  Building ASP.NET Core Web APIs with Clean Architecture - https://fullstackmark.com/post/18/building-aspnet-core-web-apis-with-clean-architecture

3.  Automapper - https://docs.automapper.org/en/stable/

4.  Facade Design Pattern In C# - https://www.c-sharpcorner.com/article/facade-design-pattern-using-c-sharp/

5.  ASP.NET Core 3.1 - Boilerplate API with Email Sign Up, Verification, Authentication & Forgot Password - https://jasonwatmore.com/post/2020/07/06/aspnet-core-3-boilerplate-api-with-email-sign-up-verification-authentication-forgot-password
 
6. How to generate a random password in .NET Core 6 with C# - https://www.shecodes.io/athena/52295-how-to-generate-a-random-password-in-net-core-6-with-c#:~:text=To%20generate%20a%20random%20password%2C%20this%20code%20uses%20the%20Random,ToArray%20methods%20from%20the%20System.
7. Hosting: https://www.accuwebhosting.com/web-hosting/windows