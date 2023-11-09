
using PS.Core.Models;
using PS.Datastore.EFCore.Interfaces;
using PS.Datastore.EFCore.Repositories;
using PS.UseCases.Interfaces;

namespace PS.UseCases.VendorUseCase
{
    public class GetAllVendorsUseCase : IGetAllVendorsUseCase
    {
        //private readonly ICountryRepository CountryRepository;
        private readonly IVendorRepository VendorRepository;

        public GetAllVendorsUseCase(IVendorRepository vendorRepository)
        {
            VendorRepository = vendorRepository;
        }

        public IQueryable<Vendor> Execute()
        {
            return VendorRepository.GetAll();
        }
    }
}
