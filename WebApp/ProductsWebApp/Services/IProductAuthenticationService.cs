using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductListWebApp.Services
{
    public interface IProductAuthenticationService
    {
         Task<AuthenticationResult> AcquireAuthenticationResult();
        bool FlushProductsAuthenticationCache();
    }
}
