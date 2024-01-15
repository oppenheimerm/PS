
using PA.Core.Models.ApiRequestResponse;
using PA.Datastore.EFCore.Interfaces;
using PA.UseCases.Interfaces;

namespace PA.UseCases.PetrolStationUseCase
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
