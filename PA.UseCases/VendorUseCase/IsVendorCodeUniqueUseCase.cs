
using PA.Datastore.EFCore.Interfaces;
using PA.UseCases.Interfaces;

namespace PA.UseCases.VendorUseCase
{
    public class IsVendorCodeUniqueUseCase : IIsVendorCodeUniqueUseCase
    {
        private readonly IVendorRepository VendorRepository;

        public IsVendorCodeUniqueUseCase(IVendorRepository vendorRepository)
        {
            VendorRepository = vendorRepository;
        }

        public async Task<bool> ExecuteAsync(string CountryCode)
        {
            var response = await VendorRepository.IsVendorCodeUnique(CountryCode);
            return response;
        }
    }
}
