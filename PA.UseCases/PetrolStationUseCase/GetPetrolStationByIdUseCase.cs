

using PA.Core.Models;
using PA.Datastore.EFCore.Interfaces;
using PA.UseCases.Interfaces;

namespace PA.UseCases.PetrolStationUseCase
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
