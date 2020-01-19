using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using ProductListWebApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductListWebApp.Services
{
    public interface IProductAuthenticationService
    {
        
        Task<IAuthenticationResultWrapper> AcquireAuthenticationResult();
        bool FlushProductsAuthenticationCache();
        UserIdentifier GetUserIdentifier();
        bool IsNaiveCached(string userId, ISession session);
        //AuthenticationContextWrapper GetAuthenticationContext();
    }
}
