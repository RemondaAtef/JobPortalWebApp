using Microsoft.AspNetCore.Identity;

namespace WebAPI.BL.Interface
{
    public interface IJWTService
    {
        Task<string> GenerateToken(IdentityUser user);
        
    }
}
