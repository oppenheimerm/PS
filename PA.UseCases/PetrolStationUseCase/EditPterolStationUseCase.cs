
using PA.Core.Models;
using PA.Datastore.EFCore.Interfaces;
using PA.UseCases.Interfaces;

namespace PA.UseCases.PetrolStationUseCase
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
