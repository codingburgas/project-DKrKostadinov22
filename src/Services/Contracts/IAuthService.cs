using Microsoft.AspNetCore.Identity;
using PharmacyManager.DTOs;

namespace PharmacyManager.Services.Contracts
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterAsync(RegisterDto model);
        Task<SignInResult> LoginAsync(LoginDto model);
        Task LogoutAsync();
    }
}
