
using PA.Core.Models;

namespace PA.Datastore.EFCore.Interfaces
{
    public interface IVendorRepository
    {
        Task<(Vendor vendor, bool Success, string ErrorMessage)> Add(Vendor vendor);
        Task<bool> IsVendorCodeUnique(string vendorCode);
        IQueryable<Vendor> GetAll();
    }
}
