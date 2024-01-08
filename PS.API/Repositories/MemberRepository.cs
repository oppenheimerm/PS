using PS.Datastore.EFCore.Repositories;
using PS.Datastore.EFCore;
using PS.Core.Models;
using Microsoft.EntityFrameworkCore;
using PS.API.Repositories.Interfaces;

namespace PS.API.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly ILogger<MembersRepository> Logger;
        private readonly ApplicationDbContext Context;     

        public MemberRepository(ILogger<MembersRepository> logger, ApplicationDbContext context,
            IWebApiUserRepository webApiUserRepository)
        {
            Logger = logger;
            Context = context;
        }


        public async Task<Member?>GetMemberById(Guid Id)
        {
            return await Context.Members
                .AsNoTracking().FirstOrDefaultAsync( m => m.Id == Id );
        }

        public async Task<(Member? member, bool Success, string ErrorMessage)>UpdateUserPhoto(Member? member, string photoName)
        {   
            if(member != null)
            {
                member.MemberPhoto = photoName;
                member.Updated = DateTime.Now;
                Context.Members.Update(member);
                await Context.SaveChangesAsync();
                Logger.LogInformation($"Member with Id: {member.Id}, updated photo at: {DateTime.UtcNow}");
                return (member, true, string.Empty);
            }
            else
            {
                Logger.LogError($"Failed to locate update member photo.  The passed in member parameter is null. Timestamp: {DateTime.UtcNow}");
                return (null, false, "Could not find user");
            }
        }
        
    }
}
