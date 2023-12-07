
using PS.Core.Models;

namespace PS.UseCases.Interfaces
{
    public interface IEditPterolStationUseCase
    {
        /// <summary>
        /// Edit a <see cref="Station"/>.
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        Task<(Station Station, bool success, string ErrorMessage)> ExecuteAsync(Station station);
    }
}
