using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PS.Core.Models;
using System;

namespace PS.Datastore.EFCore
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider, 
            string initUser, string initUserPassword)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Initial initial user
                var adminID = await EnsureUser(serviceProvider, initUser, initUserPassword);
                

                //  Add Roles to database
                await EnsureRole(serviceProvider, adminID, Core.Authorization.Constants.EmployeeRole);
                await EnsureRole(serviceProvider, adminID, Core.Authorization.Constants.StationsReadRole);
                await EnsureRole(serviceProvider, adminID, Core.Authorization.Constants.StationsCreateEditRole);
                await EnsureRole(serviceProvider, adminID, Core.Authorization.Constants.VendorsReadRole);
                await EnsureRole(serviceProvider, adminID, Core.Authorization.Constants.VendorsCreateEditRole);

                var seeded = SeedDB(context, adminID);
                if (seeded)
                {
                    await EnsureInitEmployee(adminID, context);
                }
            }

        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
                                            string initUser, string initPassword)
        {
            var userManager = serviceProvider.GetService<UserManager<Person>>();

            var user = await userManager.FindByNameAsync(initUser);
            if (user == null)
            {
                user = new Person
                {
                    UserName = initUser,
                    EmailConfirmed = true,
                    LastLogin = DateTime.UtcNow,
                    DOB = new DateTime(1971, 09, 20),
                    Email = "michieloppenheimer@pertolsist.com"
                };
                await userManager.CreateAsync(user, initPassword);
            }

            if (user == null)
            {
                throw new Exception("The password is probably not strong enough!");
            }

            return user.Id;
        }

        private static async Task EnsureInitEmployee(string id, ApplicationDbContext context)
        {
            var vserId = Guid.Parse(id);
            var Employee = new Employee()
            {
                FirstName = "Michiel",
                LasttName = "Oppenheimer",
                Initials = "L, B",
                PrimaryDepartment = PrimaryDepartment.IT_ENGINEERING,
                Title = "Mr",
            };

            Employee.Id = vserId;

            await context.Employees.AddAsync(Employee);
            await context.SaveChangesAsync();
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
                                                              string uid, string role)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            IdentityResult IR;
            if (!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<Person>>();

            //if (userManager == null)
            //{
            //    throw new Exception("userManager is null");
            //}

            var user = await userManager.FindByIdAsync(uid);

            if (user == null)
            {
                throw new Exception("The testUserPw password was probably not strong enough!");
            }

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }

        public static bool SeedDB(ApplicationDbContext context, string initialUserId)
        {
            if (context.PetrolVendors.Any())
            {
                return false;   // DB has been seeded
            }

            var countries = new Country[]
            {
                new Country { CountryName = "England", CountryCode = "ENG"},
                new Country { CountryName = "Scotland", CountryCode = "SCT"},
                new Country { CountryName = "Wales", CountryCode = "WLS"}
            };

            context.Countries.AddRange(countries);
            context.SaveChanges();

            var vendors = new Vendor[] {
                new Vendor { VendorName = "ASDA Petrol", VendorAddress = "Asda House South Bank Great Wilson Street Leeds", VendorAddress2 = "", VendorPostcode = "LS11 5AD", CountryId = 1, Logo = string.Empty, VendorCode = "ASDA", VendorLogo = "asda" },
                new Vendor { VendorName = "Shell UK", VendorAddress = "Shell Centre London", VendorAddress2 = "", VendorPostcode = "SE1 7NA", CountryId = 1, Logo = string.Empty, VendorCode = "SHUK", VendorLogo = "shell" },
                new Vendor { VendorName = "BP UK", VendorAddress = "1 St James's Square, St. James's, London", VendorAddress2 = "", VendorPostcode = "SW1Y 4PD", CountryId = 1, Logo = string.Empty, VendorCode = "BPUK", VendorLogo = "bp" },
                new Vendor { VendorName = "Texaco UK", VendorAddress = "Va​lero Energy Ltd 1 Canada Square​ London", VendorAddress2 = "", VendorPostcode = "E14 5AA", CountryId = 1, Logo = string.Empty, VendorCode = "TXUK", VendorLogo = "texaco" },
            };

            context.PetrolVendors.AddRange(vendors);
            context.SaveChanges();

            var stations = new Station[]
            {
                new Station { StationName = "Asda Bethnal Green Vallance Road Petrol Filling Station", StationAddress = "112 Vallance Rd, London", StationAddress2 = "", StationPostcode = "E1 5BW", Latitude = 51.522136, Longitude = -0.0639825, VendorId = 1, CountryId = 1},
                new Station { StationName = "Shell - Whitechapel Rd", StationAddress = "139-149 Whitechapel Rd, London", StationAddress2 = "", StationPostcode = "E1 1DT", Latitude = 51.517756000000013, Longitude = -0.066128499999999993, VendorId = 2, CountryId = 1},
                new Station { StationName = "BP Cambridge Heath Rd", StationAddress = "319 Cambridge Heath Rd, London", StationAddress2 = "", StationPostcode = "E2 9LH", Latitude = 51.5286397, Longitude = -0.055950200000000012, VendorId = 3, CountryId = 1},
                new Station { StationName = "Texaco, ", StationAddress = "ST KATHERINES, 77-101 The Hwy, London", StationAddress2 = "", StationPostcode = "E1W 2BN", Latitude = 51.5095198, Longitude = -0.06346539999999999 , VendorId = 4, CountryId = 1},
                new Station { StationName = "Shell Old Street", StationAddress = "198-208 Old St, London", StationAddress2 = "", StationPostcode = "EC1V 9BP", Latitude = 51.525169 , Longitude = -0.0904657, VendorId = 2, CountryId = 1},
                new Station { StationName = "Texaco Grove Road", StationAddress = "51, 53 Grove Rd., Bow, London", StationAddress2 = "", StationPostcode = "E3 5DU", Latitude = 51.52735149999999, Longitude = -0.036384, VendorId = 4, CountryId = 1},
                new Station { StationName = "BP", StationAddress = "102-106 The Hwy, London", StationAddress2 = "", StationPostcode = "E1W 2BU", Latitude = 51.5092828, Longitude = -0.06055000000000001, VendorId = 3, CountryId = 1},
                new Station { StationName = "Texaco", StationAddress = "241 City Rd, London", StationAddress2 = "", StationPostcode = "EC1V 1JQ", Latitude = 51.5297989, Longitude = -0.0948134, VendorId = 4, CountryId = 1},
            };
            context.PetrolStations.AddRange(stations);
            context.SaveChanges();

            return true;
        }
    }
}
