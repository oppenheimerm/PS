using PS.Core.Models;
using PS.Datastore.EFCore.Interfaces;
using PS.UseCases.Interfaces;

namespace PS.UseCases.PetrolStationUseCase
{
    public class GetPetrolStationByIdUseCase : IGetPetrolStationByIdUseCase
    {
        private readonly IPetrolStationRepository StationRepository;

        public GetPetrolStationByIdUseCase(IPetrolStationRepository stationRepository)
        {
            StationRepository = stationRepository;
        }

        public async Task<Station?> ExecuteAsync(int id)
        {
            return await StationRepository.GetStationById(id);
        }
    }
}
