
namespace PA.UseCases.Interfaces
{
    public interface IIsVendorCodeUniqueUseCase
    {
        Task<bool> ExecuteAsync(string CountryCode);
    }
}
