
using System.ComponentModel.DataAnnotations;

namespace PA.Core.Models
{
    public class CustomerHistory
    {

        [Key]
        public int Id { get; set; }
        public DateTime? Date { get; set; }

        public Member? Member { get; set; }
        [Required]
        public Guid? MemberId { get; set; }

        public Station? Station { get; set; }
        [Required]
        public int? StationId { get; set; }
    }
}
