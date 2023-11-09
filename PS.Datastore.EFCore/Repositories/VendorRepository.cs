
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PS.Core.Models;
using PS.Datastore.EFCore.Interfaces;

namespace PS.Datastore.EFCore.Repositories
{
    [Authorize]
    public class VendorRepository : IVendorRepository
    {
        private readonly ILogger<VendorRepository> Logger;
        private readonly ApplicationDbContext Context;

        public VendorRepository(ILogger<VendorRepository> logger, ApplicationDbContext contenxt)
        {
            Logger = logger;
            Context = contenxt;
        }

        public IQueryable<Vendor> GetAll()
        {
            var vendors = Context.PetrolVendors
                .Include(c => c.Country)
                .AsNoTracking();
            return vendors; ;
        }

        public async Task<(Vendor vendor, bool Success, string ErrorMessage)> Add(Vendor vendor)
        {
            // unique vendor code?
            var isUnique = await IsVendorCodeUnique(vendor.VendorCode);
            if (isUnique)
            {
                try
                {
                    Context.PetrolVendors.Add(vendor);
                    await Context.SaveChangesAsync();
                    Logger.LogInformation($"Vendor with Id: {vendor.Id}, and vendor code of: {vendor.VendorCode}added to database at: {DateTime.UtcNow}");
                    return (vendor, true, string.Empty);
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Failed to add vendor to database. Timestamp : {DateTime.UtcNow}");
                    return (vendor, false, ex.ToString());
                }
            }
            else
            {
                var errMsg = $"Failed to add vendor with vendor code {vendor.VendorCode} to database.  Code already in use. Timestamp : {DateTime.UtcNow}";
                Logger.LogError(errMsg);
                return (vendor, false, errMsg);
            }

        }


        // Helpers
        public async Task<bool> IsVendorCodeUnique(string vendorCode)
        {
            var codeInUse = await Context.PetrolVendors
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.VendorCode == vendorCode.ToUpper());

            return (codeInUse == null) ? true : false;
        }
    }
}
