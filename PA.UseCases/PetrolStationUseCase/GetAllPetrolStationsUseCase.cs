
using PA.Core.Models;
using PA.Datastore.EFCore.Interfaces;
using PA.UseCases.Interfaces;

namespace PA.UseCases.PetrolStationUseCase
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
