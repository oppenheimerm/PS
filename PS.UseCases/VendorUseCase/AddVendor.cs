using PS.Core.Models;
using PS.Datastore.EFCore;
using PS.Datastore.EFCore.Interfaces;
using PS.UseCases.Interfaces;

namespace PS.UseCases.VendorUseCase
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
