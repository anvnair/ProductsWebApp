#region Namespaces
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using ProductListWebApp.Utils;
using System.Threading.Tasks;
#endregion


/// <summary>
/// Product List WebApp Services
/// </summary>
namespace ProductListWebApp.Services
{
    public interface IProductAuthenticationService
    {
        /// <summary>Acquires the authentication result.</summary>
        /// <returns></returns>
        Task<IAuthenticationResultWrapper> AcquireAuthenticationResult();
        bool FlushProductsAuthenticationCache();
        UserIdentifier GetUserIdentifier();
        bool IsNaiveCached(string userId, ISession session);
        //AuthenticationContextWrapper GetAuthenticationContext();
    }
}
