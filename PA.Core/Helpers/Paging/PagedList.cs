
namespace PA.Core.Helpers.Paging
{
	/// <summary>
	/// Provides paging facilities for our repository layer.  Inherits from
	/// <see cref="List{T}"/> class.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class PagedList<T> : List<T>
	{
		public int CurrentPage { get; private set; }
		/// <summary>
		/// TotalPages is calculated by dividing the number of items by the page 
		/// size and then rounding it to the larger number since a page needs to 
		/// exist even if there is just one item on it.
		/// </summary>
		public int TotalPages { get; private set; }
		public int PageSize { get; private set; }
		public int TotalCount { get; private set; }

		/// <summary>
		/// HasPrevious is true if <see cref="CurrentPage"/> is larger than 1.
		/// </summary>
		public bool HasPrevious => CurrentPage > 1;
		/// <summary>
		/// HasNext is calculated if <see cref="CurrentPage"/> is smaller than the number of total pages.
		/// </summary>
		public bool HasNext => CurrentPage < TotalPages;

		public PagedList(List<T> items, int count, int pageNumber, int pageSize)
		{
			TotalCount = count;
			PageSize = pageSize;
			CurrentPage = pageNumber;
			TotalPages = (int)Math.Ceiling(count / (double)pageSize);

			AddRange(items);
		}

		public static PagedList<T> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
		{
			var count = source.Count();
			var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

			return new PagedList<T>(items, count, pageNumber, pageSize);
		}
	}
}
