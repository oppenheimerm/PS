
using PS.Core.Models;
using PS.Datastore.EFCore.Interfaces;
using PS.UseCases.Interfaces;

namespace PS.UseCases.PetrolStationUseCase
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
