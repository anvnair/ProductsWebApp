using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Core;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using ProductWebApp;
using System.Security.Claims;
using System.Threading;


namespace ProductListWebApp.Services
{
    public class ProductAuthenticationService : IProductAuthenticationService
    {
        public ProductAuthenticationService(IHttpContextAccessor context)
        {
            _context = context;

        }
        private readonly IHttpContextAccessor _context;

        public ISession _session { get; private set; }

        public async Task<AuthenticationResult> AcquireAuthenticationResult()
        {
            // To fetch the already logged in user object
            var claim = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string userObjectID = _context.HttpContext.User != null ? (_context.HttpContext.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier"))?.Value : "1";
            
            // Using ADAL.Net, get a bearer token to access the ProductService
            AuthenticationContext authContext = new AuthenticationContext(AzureAdOptions.Settings.Authority, new NaiveSessionCache(userObjectID, _context.HttpContext.Session));
            ClientCredential credential = new ClientCredential(AzureAdOptions.Settings.ClientId, AzureAdOptions.Settings.ClientSecret);
        
            //var result = await authContext.AcquireTokenAsync(AzureAdOptions.Settings.ProductResourceId, credential);
            var result = await authContext.AcquireTokenSilentAsync(AzureAdOptions.Settings.ProductResourceId, credential, new UserIdentifier(userObjectID, UserIdentifierType.UniqueId));
            return result;
        }

        public bool FlushProductsAuthenticationCache()
        {
            bool isSuccess = true;

            try
            {
                string userObjectID = _context.HttpContext.User != null ? (_context.HttpContext.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier"))?.Value : "1";
                AuthenticationContext authContext = new AuthenticationContext(AzureAdOptions.Settings.Authority, new NaiveSessionCache(userObjectID, _context.HttpContext.Session));
                var ProductTokens = authContext.TokenCache.ReadItems().Where(a => a.Resource == AzureAdOptions.Settings.ProductResourceId);
                foreach (TokenCacheItem tci in ProductTokens)
                    authContext.TokenCache.DeleteItem(tci);
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }
            return isSuccess;
        }
    }
}
