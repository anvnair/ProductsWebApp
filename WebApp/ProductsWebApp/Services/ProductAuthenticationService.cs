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

        public AuthenticationContextWrapper authContextWrapperObject { get; set; }
        public bool isNaiveCached { get; set; }

        public ProductAuthenticationService(IHttpContextAccessor context, IAzureAD ad, INaiveSessionCache naiveCache, IAuthenticationContextWrapper authWrapper)
        {
            _context = context;
            _ad = ad;
            _adSettings = _ad.InitAzureSettings();
            _userObjectID = _ad.GetUserID(_context);
            _naiveCache = naiveCache;         
            _authWrapper = authWrapper;
        }
        public bool IsNaiveCached(string userId,ISession session)
        {
            return _naiveCache.SetUpNaiveSessionCache(userId, _context.HttpContext.Session);
        }
        private readonly IHttpContextAccessor _context;
        private readonly IAzureAD _ad;
        public AzureAdOptions _adSettings;
        private string _userObjectID;
        private INaiveSessionCache _naiveCache;
        private IAuthenticationContextWrapper _authWrapper;

        public ISession _session { get; private set; }

        public AuthenticationContextWrapper GetAuthContext()
        {
            return new AuthenticationContextWrapper();
        }
        public async Task<IAuthenticationResultWrapper> AcquireAuthenticationResult()
        {
            // To fetch the already logged in user object
            var claim = (ClaimsPrincipal)Thread.CurrentPrincipal;            
            isNaiveCached = IsNaiveCached(userObjectID, _context.HttpContext.Session);
            this.authContextWrapperObject = _authWrapper.SetAuthenticationContext(_adSettings.Authority, _naiveCache.SetUpNaiveSessionCache(userObjectID, _context.HttpContext.Session));
            ClientCredential credential = new ClientCredential(_adSettings.ClientId, _adSettings.ClientSecret);

            var result = await authContextWrapperObject.AcquireTokenSilentAsync(_adSettings.ProductResourceId, credential, GetUserIdentifier());
            return result;
        }

        public UserIdentifier GetUserIdentifier()
        {
            return new UserIdentifier(this.userObjectID, UserIdentifierType.UniqueId);
        }

        public bool FlushProductsAuthenticationCache()
        {
            bool isSuccess = true;

            try
            {
                AuthenticationContext authContext = new AuthenticationContext(AzureAdOptions.Settings.Authority, isNaiveCached);
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
