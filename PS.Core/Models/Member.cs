
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace PS.Core.Models
{
    public class Member
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public Guid Id { get; set; }
        [Required]
        [StringLength(30)]
        [PersonalData]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(7)]
        public string? Title { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        [PersonalData]
        public string LasttName { get; set; } = string.Empty;

        [StringLength(8)]
        [PersonalData]
        public string? Initials { get; set; } = string.Empty;


        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set;} = string.Empty;

        [JsonIgnore]
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        /// <summary>
        /// Verification date of emaail
        /// </summary>
        [JsonIgnore]
        public DateTime? Verified { get; set; }
        [JsonIgnore]
        public bool IsVerified => Verified.HasValue || PasswordReset.HasValue;
        [JsonIgnore]
        public string? ResetToken { get; set; }
        [JsonIgnore]
        public DateTime? ResetTokenExpires { get; set; }
        [JsonIgnore]
        public DateTime? PasswordReset { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public string? MobileNumber { get; set; }

        /// <summary>
        /// Distance in miles(0) / kilometers(1). Default 1
        /// </summary>
        public int DistanceUnit { get; set; } = 1; // Kilometers

        [JsonIgnore]
        public List<RefreshToken>? RefreshTokens { get; set; }

        public bool AcceptTerms { get; set; } = false;
        [JsonIgnore]
        public string VerificationToken { get; set; } = string.Empty;

        public string MemberPhoto { get; set; } = string.Empty;

        //https://github.com/cornflourblue/aspnet-core-3-signup-verification-api/blob/master/Entities/Account.cs
        public DateTime? RegisteredDate { get; set; } = DateTime.Now;
        public DateTime? LastLogIn { get; set; }
    }
}
