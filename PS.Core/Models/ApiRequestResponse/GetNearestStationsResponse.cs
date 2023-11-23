
using PS.Core.Helpers.Paging;

namespace PS.Core.Models.ApiRequestResponse
{
    public class GetNearestStationsResponse
    {
        public List<StationLite>? Stations { get; set; }
        public int? TotalCount { get; set; }
        public int? PageSize { get; set; }
        public int? CurrentPage { get; set; }
        public int? TotalPages { get; set; }
        public bool HasNext { get; set; } = false;
        public bool HasPrevious { get; set; } = false;

    }

}
