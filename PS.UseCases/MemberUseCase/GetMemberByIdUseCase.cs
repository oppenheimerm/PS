
using PS.Core.Models;
using PS.Datastore.EFCore.Interfaces;
using PS.UseCases.Interfaces;

namespace PS.UseCases.MemberUseCase
{
    public class GetMemberByIdUseCase : IGetMemberByIdUseCase
    {
        public readonly IMembersRepository MembersRepository;

        public GetMemberByIdUseCase(IMembersRepository membersRepository)
        {
            MembersRepository = membersRepository;
        }

        
        public async Task<Member?> ExecuteAsync(Guid id)
        {
            return await MembersRepository.GetMember(id);
        }

    }
}
