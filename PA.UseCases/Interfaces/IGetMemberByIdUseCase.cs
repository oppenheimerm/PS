
using PA.Core.Models;

namespace PA.UseCases.Interfaces
{
    public interface IGetMemberByIdUseCase
    {
        /// <summary>
        /// Get member by id(<see cref="Guid"/>)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Member?> ExecuteAsync(Guid id);
    }
}
