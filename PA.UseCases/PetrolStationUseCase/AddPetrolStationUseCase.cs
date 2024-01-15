
using PA.Core.Models;
using PA.Datastore.EFCore.Interfaces;
using PA.UseCases.Interfaces;

namespace PA.UseCases.PetrolStationUseCase
{
    public class AddPetrolStationUseCase : IAddPetrolStationUseCase
    {
        //private readonly IVendorRepository VendorRepository;
        private readonly IPetrolStationRepository PetrolStationRepository;

        public AddPetrolStationUseCase(IPetrolStationRepository petrolStationRepository)
        {
            PetrolStationRepository = petrolStationRepository;
        }

        public async Task<(Station Station, bool success, string ErrorMessage)> ExecuteAsync(Station station)
        {
            var response = await PetrolStationRepository.Add(station);
            return response;
        }
    }
}
