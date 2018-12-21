using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace HostMVC.Services
{
    public interface ILoginService<T> where T : IdentityUser
    {
        Task<bool> ValidateCredentials(T user, string password);
        Task<T> FindByUsername(string username);
        Task SignIn(T user);
    }
}
