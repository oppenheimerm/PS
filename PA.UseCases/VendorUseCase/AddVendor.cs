
using PA.Core.Models;
using PA.Datastore.EFCore.Interfaces;
using PA.UseCases.Interfaces;

namespace PA.UseCases.VendorUseCase
{
    public class AddVendor : IAddVendorUseCase
    {
        private readonly IVendorRepository VendorRepository;

        public AddVendor(IVendorRepository vendorRepository)
        {
            VendorRepository = vendorRepository;
        }
        public async Task<(Vendor Vendor, bool success, string ErrorMessage)> ExecuteAsync(Vendor vendor)
        {
            var response = await VendorRepository.Add(vendor);
            return response;
        }
    }
}
