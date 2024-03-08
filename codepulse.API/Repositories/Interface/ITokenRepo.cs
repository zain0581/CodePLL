using Microsoft.AspNetCore.Identity;

namespace codepulse.API.Repositories.Interface
{
    public interface ITokenRepo
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
