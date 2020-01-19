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
using ProductListWebApp.Utils;

namespace ProductListWebApp.Services
{
    public class ProductAuthenticationService : IProductAuthenticationService
    {
        public string userObjectID
        {
            get
            {
                return _userObjectID;
            }
            set
            {
                _userObjectID = value;
            }
        }

        public AuthenticationContextWrapper authContext { get; set; }
        public ProductAuthenticationService(IHttpContextAccessor context, IAzureAD ad, INaiveSessionCache naiveCache, IAuthenticationContextWrapper authWrapper)
        {
            _context = context;
            _ad = ad;
            _adSettings = _ad.InitAzureSettings();
            _userObjectID = _ad.GetUserID(_context);
            _naiveCache = naiveCache;
            _authWrapper = authWrapper;
        }
        private readonly IHttpContextAccessor _context;
        private readonly IAzureAD _ad;
        public AzureAdOptions _adSettings;
        private string _userObjectID;
        private INaiveSessionCache _naiveCache;
        private IAuthenticationContextWrapper _authWrapper;

        public ISession _session { get; private set; }

        public async Task<IAuthenticationResultWrapper> AcquireAuthenticationResult()
        {
            // To fetch the already logged in user object
            var claim = (ClaimsPrincipal)Thread.CurrentPrincipal;
            // Using ADAL.Net, get a bearer token to access the ProductService
            //this.authContext = new AuthenticationContextWrapper(_adSettings.Authority, _naiveCache.SetUpNaiveSessionCache(userObjectID, _context.HttpContext.Session));
            this.authContext = _authWrapper.SetAuthenticationContext(_adSettings.Authority, _naiveCache.SetUpNaiveSessionCache(userObjectID, _context.HttpContext.Session));
           
            ClientCredential credential = new ClientCredential(_adSettings.ClientId, _adSettings.ClientSecret);

            var result = await authContext.AcquireTokenSilentAsync(_adSettings.ProductResourceId, credential, new UserIdentifier(userObjectID, UserIdentifierType.UniqueId));
            return result;
        }

        public bool FlushProductsAuthenticationCache()
        {
            bool isSuccess = true;

            try
            {
                AuthenticationContext authContext = new AuthenticationContext(AzureAdOptions.Settings.Authority, _naiveCache.SetUpNaiveSessionCache(userObjectID, _context.HttpContext.Session));
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
