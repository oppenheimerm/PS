
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PA.Core.Models;
using PA.Datastore.EFCore.Interfaces;

namespace PA.Datastore.EFCore.Repositories
{
    public class MembersRepository : IMembersRepository
    {
        private readonly ILogger<MembersRepository> Logger;
        private readonly ApplicationDbContext Context;

        public MembersRepository(ILogger<MembersRepository> logger, ApplicationDbContext context)
        {
            Logger = logger;
            Context = context;
        }



        public IQueryable<Member> GetAll()
        {
            return Context.Members.AsNoTracking().OrderBy(m => m.LasttName);
        }

        public async Task<Member?> GetMember(Guid id)
        {
            return await Context.Members.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Member?> GetMemberById(Guid Id)
        {
            return await Context.Members
                .AsNoTracking().FirstOrDefaultAsync(m => m.Id == Id);
        }

        public async Task<(Member? member, bool Success, string ErrorMessage)> UpdateUserPhoto(Member? member, string photoName)
        {
            if (member != null)
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
