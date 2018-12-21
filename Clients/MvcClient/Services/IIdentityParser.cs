using Microsoft.AspNetCore.Identity;

namespace MvcClient.Services
{
    public interface IIdentityParser<T> where T : IdentityUser
    {
    }
}
