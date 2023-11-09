using PS.Core.Models;

namespace PS.Datastore.EFCore.Interfaces
{
    public interface IVendorRepository
    {
        Task<(Vendor vendor, bool Success, string ErrorMessage)> Add(Vendor vendor);
        Task<bool> IsVendorCodeUnique(string vendorCode);
        IQueryable<Vendor> GetAll();


    }
}
