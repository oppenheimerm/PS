using PS.Core.Models;

namespace PS.UseCases.Interfaces
{
    public interface IAddVendorUseCase
    {
        Task<(Vendor Vendor, bool success, string ErrorMessage)> ExecuteAsync(Vendor vendor);
    }
}
