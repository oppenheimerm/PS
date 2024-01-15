
using PA.Core.Models;

namespace PA.UseCases.Interfaces
{
    public interface IAddVendorUseCase
    {
        Task<(Vendor Vendor, bool success, string ErrorMessage)> ExecuteAsync(Vendor vendor);
    }
}
