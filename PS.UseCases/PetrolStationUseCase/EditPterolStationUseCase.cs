
using PS.Core.Models;
using PS.Datastore.EFCore.Interfaces;
using PS.UseCases.Interfaces;

namespace PS.UseCases.PetrolStationUseCase
{
    public class EditPterolStationUseCase : IEditPterolStationUseCase
    {
        private readonly IPetrolStationRepository PetrolStationRepository;

        public EditPterolStationUseCase(IPetrolStationRepository petrolStationRepository)
        {
            PetrolStationRepository = petrolStationRepository;
        }

        public async Task<(Station Station, bool success, string ErrorMessage)> ExecuteAsync(Station station)
        {
            var response = await PetrolStationRepository.Edit(station);
            return response;
        }
    }
}
