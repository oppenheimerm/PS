
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PS.Core.Models;
using PS.Datastore.EFCore.Interfaces;

namespace PS.Datastore.EFCore.Repositories
{
    public class MembersRepository : IMembersRepository
    {
        private readonly ILogger<MembersRepository> Logger;
        private readonly ApplicationDbContext Context;

        public MembersRepository( ILogger<MembersRepository> logger, ApplicationDbContext context)
        {
            Logger = logger;
            Context = context;
        }



        public IQueryable<Member> GetAll()
        {
            return Context.Members.AsNoTracking().OrderBy( m => m.LasttName );
        }

        public async Task<Member?> GetMember(Guid id)
        {
            return await Context.Members.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
