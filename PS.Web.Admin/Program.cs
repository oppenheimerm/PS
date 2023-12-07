using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PS.Core.Models;
using PS.Datastore.EFCore;
using PS.Datastore.EFCore.Interfaces;
using PS.Datastore.EFCore.Repositories;
using PS.UseCases.CountryUseCase;
using PS.UseCases.EmployeeUseCase;
using PS.UseCases.Interfaces;
using PS.UseCases.PetrolStationUseCase;
using PS.UseCases.VendorUseCase;
using PS.Web.Admin.Hubs;
using System;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<Person>(options => options.SignIn.RequireConfirmedAccount = true)
	//Add Role services to Identity
	.AddRoles<IdentityRole>()
	.AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();

//  Require authenticated users
builder.Services.AddAuthorization(options =>
{
	options.FallbackPolicy = new AuthorizationPolicyBuilder()
		.RequireAuthenticatedUser()
		.Build();
});

// Authorization handlers.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<IdentityOptions>(options =>
{
	// Password settings.
	options.Password.RequireDigit = true;
	options.Password.RequireLowercase = true;
	options.Password.RequireNonAlphanumeric = true;
	options.Password.RequireUppercase = true;
	options.Password.RequiredLength = 6;
	options.Password.RequiredUniqueChars = 1;

	// Lockout settings.
	options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
	options.Lockout.MaxFailedAccessAttempts = 5;
	options.Lockout.AllowedForNewUsers = true;

	// User settings.
	options.User.AllowedUserNameCharacters =
	"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
	options.User.RequireUniqueEmail = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
	// Cookie settings
	options.Cookie.HttpOnly = true;
	options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

	options.LoginPath = "/Identity/Account/Login";
	options.AccessDeniedPath = "/Identity/Account/AccessDenied";
	options.SlidingExpiration = true;
});

//  Fix error: System.Text.Json.JsonException: A possible object cycle was detected. This can either be due
//  to a cycle or if the object depth is larger than the maximum allowed depth of 32. Consider
//  using ReferenceHandler.Preserve on JsonSerializerOptions to support cycles.
//
//  nuget package: Microsoft.AspNetCore.Mvc.NewtonsoftJson
builder.Services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


//  Repositories
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IVendorRepository, VendorRepository>();
builder.Services.AddScoped<IPetrolStationRepository, PetrolStationRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

//  Usercases
builder.Services.AddTransient<IAddCountryUseCase, AddCountry>();
builder.Services.AddTransient<IGetAllCountriesUseCase, GetAllCountriesUseCase>();
builder.Services.AddTransient<IAddVendorUseCase, AddVendor>();
builder.Services.AddTransient<IGetCountryUseCase, GetCountryUseCase>();
builder.Services.AddTransient<IEditCountryUseCase, EditCountryUseCase>();
builder.Services.AddTransient<IIsCountryCodeUniqueUseCase, IsCountryCodeUniqueUseCase>();
builder.Services.AddTransient<IIsVendorCodeUniqueUseCase, IsVendorCodeUniqueUseCase>();
builder.Services.AddTransient<IGetAllVendorsUseCase, GetAllVendorsUseCase>();
builder.Services.AddTransient<IAddPetrolStationUseCase, AddPetrolStationUseCase>();
builder.Services.AddTransient<IGetAllPetrolStationsUseCase, GetAllPetrolStationsUseCase>();
builder.Services.AddTransient<IGetCountryCodeByIdUseCase, GetCountryCodeByIdUseCase>();
builder.Services.AddTransient<IGetAllPetrolStationsFlatUseCase, GetAllPetrolStationsFlatUseCase>();
builder.Services.AddTransient<IGetCountryCodeByCodeUseCase, GetCountryCodeByCodeUseCase>();
builder.Services.AddTransient<IGetPetrolStationByIdUseCase, GetPetrolStationByIdUseCase>();
builder.Services.AddTransient<IGetEmployeesDirectoryUseCase, GetEmployeesDirectoryUseCase>();
builder.Services.AddTransient<IAddEmployeeUseCase, AddEmployeeUseCase>();
builder.Services.AddTransient<IAddEmployeeObjectUseCase, AddEmployeeObjectUseCase>();
builder.Services.AddTransient<IGetAllStationNearLatLongPoint, GetAllStationNearLatLongPoint>();
builder.Services.AddTransient<IEditPterolStationUseCase, EditPterolStationUseCase>();
//builder.Services.AddTransient<,>();


builder.Services.AddSignalR();



var app = builder.Build();


//  The EnsureCreated method takes no action if a database for the context exists.
//  If no database exists, it creates the database and schema. EnsureCreated
//  enables the following workflow for handling data model changes:
using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;

	var context = services.GetRequiredService<ApplicationDbContext>();

	//context.Database.EnsureCreated();
	context.Database.Migrate();

	var initUser = builder.Configuration.GetValue<string>("InitialUser:UserName");
	var initPassword = builder.Configuration.GetValue<string>("InitialUser:Password");

	await DbInitializer.Initialize(services, initUser, initPassword);
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//  Identity is enabled by calling UseAuthentication. UseAuthentication adds authentication middleware to the request
//  pipeline. app.UseAuthorization is included to ensure it's added in the correct order should the app add
//  authorization.
//      UseRouting,
//      UseAuthentication,
//      and UseAuthorization
//  must be called in the order above.
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapHub<NotificationHub>("/notificationHub");

app.Run();
