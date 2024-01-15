using PA.Core.Models;
using PA.Core.Models.ApiRequestResponse;

namespace PA.Web.API.ModelHelpers
{
    public static class ModelExtensions
    {

        public static Member ToMember(this RegisterRequest registerRequest)
        {
            if (registerRequest == null) throw new ArgumentNullException(nameof(registerRequest));
            else
            {
                return new Member
                {
                    Id = Guid.NewGuid(),
                    FirstName = registerRequest.FirstName,
                    LasttName = registerRequest.LastName,
                    EmailAddress = registerRequest.EmailAddress.ToLowerInvariant(),
                    AcceptTerms = registerRequest.AcceptTerms,
                    MobileNumber = registerRequest.MobileNumber
                };
            }
        }
    }
}
