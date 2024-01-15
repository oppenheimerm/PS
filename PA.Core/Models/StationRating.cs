
using System.ComponentModel.DataAnnotations;

namespace PA.Core.Models
{
	public class StationRating
	{

		[Key]
		public int Id { get; set; }

		public DateTime Date { get; set; } = DateTime.Now;

		[Required]
		public int StationId { get; set; }
		public Station? Station { get; set; }

		[Required]
		public Guid MemberId { get; set; }
		public Member? Member { get; set; }

		[Required]
		double? Rating { get; set; }


		[StringLength(200, ErrorMessage = "Review can't be longer than 250 characters.")]
		public string? Review { get; set; }
	}
}
