
using PA.Core.Models.ApiRequestResponse;

namespace PA.UseCases.Interfaces
{
    public interface IGetAllPetrolStationsFlatUseCase
    {
        /// <summary>
        /// Usecase for returning a list of <see cref="StationLite"/> with subset of
        /// <see cref="Core.Models.Station"/>properties only
        /// </summary>
        /// <returns></returns>
        IQueryable<StationLite> Execute();
    }
}
