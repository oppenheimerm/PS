
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
        public DateTime? Verified { get; set; }
        public bool IsVerified => Verified.HasValue || PasswordReset.HasValue;
        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public DateTime? PasswordReset { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

        [JsonIgnore]
        public List<RefreshToken>? RefreshTokens { get; set; }
        [JsonIgnore]
        public bool AcceptTerms { get; set; }
        public string VerificationToken { get; set; } = string.Empty;

        public string MemberPhoto { get; set; } = string.Empty;

        //https://github.com/cornflourblue/aspnet-core-3-signup-verification-api/blob/master/Entities/Account.cs
        public DateTime? RegisteredDate { get; set; } = DateTime.Now;
        public DateTime? LastLogIn { get; set; }
    }
}
