using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PS.API.Authorization.Interfaces;
using PS.API.Authorization;
using PS.Datastore.EFCore;
using PS.API.Repositories.Interfaces;
using PS.API.Repositories;
using PS.UseCases.Interfaces;
using PS.UseCases.PetrolStationUseCase;
using PS.Datastore.EFCore.Interfaces;
using PS.Datastore.EFCore.Repositories;
using System.Text.Json.Serialization;
using PS.API.Authorization.MiddleWare;
using PS.API.Filters;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.FileProviders;
using Asp.Versioning;
using PS.API.Helpers;
var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];


// Add services to the container.

//builder.Services.AddControllers();//02/11/2023
//  Action Filters
//
//  If we want to use our filter globally, we need to register it inside the AddControllers()
//  method in the ConfigureServices method.  In .NET 6 and above, we don’t have the Startup
//  class, so we have to use the Program class:
//  https://code-maze.com/action-filters-aspnetcore/
builder.Services.AddControllers(config => { 
    config.Filters.Add(new ValidateModelAttribute());
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
//  Add nuget Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore to PS.Datastore.EFCore
//  https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.databasedeveloperpageexceptionfilterserviceextensions.adddatabasedeveloperpageexceptionfilter?view=aspnetcore-7.0
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddCors();
builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);

builder.Services.AddApiVersioning(x =>
{
	x.DefaultApiVersion = new ApiVersion(1, 0);
	x.AssumeDefaultVersionWhenUnspecified = true;
	x.ReportApiVersions = true;
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//  02/11/2023
//  https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-7.0&tabs=visual-studio
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "PetrolSist User Web API",
        Description = "PetrolSist user API for mobile access ",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });
	//https://www.c-sharpcorner.com/article/how-to-add-jwt-bearer-token-authorization-functionality-in-swagger/
	options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter yourJWt token in the text input below.",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


//  Fix error: System.Text.Json.JsonException: A possible object cycle was detected. This can either be due
//  to a cycle or if the object depth is larger than the maximum allowed depth of 32. Consider
//  using ReferenceHandler.Preserve on JsonSerializerOptions to support cycles.
//
//  nuget package: Microsoft.AspNetCore.Mvc.NewtonsoftJson
//builder.Services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// configure DI for application services
builder.Services.AddScoped<IJwtUtils, JwtUtils>();

builder.Services.AddSingleton<ImageProcessor>();


//  Repositories
builder.Services.AddScoped<IPetrolStationRepository, PetrolStationRepository>();
builder.Services.AddScoped<IWebApiUserRepository, WebApiUserRepository>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
//builder.Services.AddTransient<,>();

//  UseCases
builder.Services.AddTransient<IGetAllPetrolStationsFlatUseCase, GetAllPetrolStationsFlatUseCase>();
builder.Services.AddTransient<IGetAllStationNearLatLongPoint, GetAllStationNearLatLongPoint>();

builder.Services.AddScoped<IPhotoFileRepository, PhotoFileRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    //https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-7.0&tabs=visual-studio
    //  To serve the Swagger UI at the app's root (https://localhost:<port>/),
    //  set the RoutePrefix property to an empty string:
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "PS.API v1");
    options.RoutePrefix = string.Empty;

});

// Enable directory browsing
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(
    Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\img\logos")),
    RequestPath = new PathString("/img/logos")
});

app.UseDirectoryBrowser(new DirectoryBrowserOptions()
{
    FileProvider = new PhysicalFileProvider(
    Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\img\logos")),
    RequestPath = new PathString("/img/logos")
});



// configure HTTP request pipeline
{
    // global cors policy
    app.UseCors(x => x
        .SetIsOriginAllowed(origin => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());

    // global error handler
    //app.UseMiddleware<ErrorHandlerMiddleware>();

    // custom jwt auth middleware
    app.UseMiddleware<JwtMiddleware>();


    app.MapControllers();
}
app.Run();
