
using PS.Core.Models.ApiRequestResponse;
using PS.Datastore.EFCore.Interfaces;
using PS.UseCases.Interfaces;

namespace PS.UseCases.PetrolStationUseCase
{
    public class GetAllPetrolStationsFlatUseCase : IGetAllPetrolStationsFlatUseCase
    {
        private readonly IPetrolStationRepository PetrolStationRepository;

        public GetAllPetrolStationsFlatUseCase(IPetrolStationRepository petrolStationRepository)
        {
            PetrolStationRepository = petrolStationRepository;
        }

        public IQueryable<StationLite> Execute()
        {
            return PetrolStationRepository.GetAllFlat();
        }
    }
}
