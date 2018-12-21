using Microsoft.AspNetCore.Identity;

namespace MvcClient_2.Services
{
    public interface IIdentityParser<T> where T : IdentityUser
    {
    }
}
