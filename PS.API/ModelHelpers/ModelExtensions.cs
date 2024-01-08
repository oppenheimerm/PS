using PS.Core.Models.ApiRequestResponse;
using PS.Core.Models;

namespace PS.API.ModelHelpers
{
    public static class ModelExtensions
    {

        public static Member ToMember(this RegisterRequest registerRequest)
        {
            if(registerRequest == null) throw new ArgumentNullException(nameof(registerRequest));
            else
            {
                return new Member
                {
                    Id = Guid.NewGuid(),
                    FirstName = registerRequest.FirstName,
                    LasttName = registerRequest.LastName,
                    Initials = registerRequest.Initlals,
                    EmailAddress = registerRequest.EmailAddress.ToLowerInvariant(),
                    Title = registerRequest.Title,
                    AcceptTerms = registerRequest.AcceptTerms,
					MobileNumber = registerRequest.MobileNumber
				};
            }
        }
    }
}
