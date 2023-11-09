
using PS.Core.Models;
using PS.Datastore.EFCore.Interfaces;
using PS.UseCases.Interfaces;

namespace PS.UseCases.PetrolStationUseCase
{
    public class GetAllPetrolStationsUseCase : IGetAllPetrolStationsUseCase
    {
        private readonly IPetrolStationRepository PetrolStationRepository;

        public GetAllPetrolStationsUseCase(IPetrolStationRepository petrolStationRepository)
        {
            PetrolStationRepository = petrolStationRepository;
        }

        public IQueryable<Station> Execute()
        {
            return PetrolStationRepository.GetAll();
        }
    }
}
