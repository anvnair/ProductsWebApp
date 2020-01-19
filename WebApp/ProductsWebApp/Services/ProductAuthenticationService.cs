#region Namespaces
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using ProductWebApp;
using System.Security.Claims;
using System.Threading;
using ProductListWebApp.Utils;
#endregion

/// <summary>
/// Product List WebApp Services
/// </summary>
namespace ProductListWebApp.Services
{
    /// <summary></summary>
    /// <seealso cref="ProductListWebApp.Services.IProductAuthenticationService"/>
    public class ProductAuthenticationService : IProductAuthenticationService
    {
        #region Properties

        /// <summary>Gets or sets the user object identifier.</summary>
        /// <value>The user object identifier.</value>
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

        /// <summary>Gets the session.</summary>
        /// <value>The session.</value>
        public ISession _session { get; private set; }
        #endregion

        /// <summary>The context</summary>
        private readonly IHttpContextAccessor _context;

        /// <summary>The ad</summary>
        private readonly IAzureAD _ad;


        /// <summary>The ad settings</summary>
        public AzureAdOptions _adSettings;

        /// <summary>The user object identifier</summary>
        private string _userObjectID;

        /// <summary>The naive cache</summary>
        private INaiveSessionCache _naiveCache;

        /// <summary>The authentication wrapper</summary>
        private IAuthenticationContextWrapper _authWrapper;

        /// <summary>Gets or sets the authentication context.</summary>
        /// <value>The authentication context.</value>
        public AuthenticationContextWrapper authContext { get; set; }


        /// <summary>Initializes a new instance of the <see cref="ProductAuthenticationService"/> class.</summary>
        /// <param name="context">The context.</param>
        /// <param name="ad">The ad.</param>
        /// <param name="naiveCache">The naive cache.</param>
        /// <param name="authWrapper">The authentication wrapper.</param>
        public ProductAuthenticationService(IHttpContextAccessor context, IAzureAD ad, INaiveSessionCache naiveCache, IAuthenticationContextWrapper authWrapper)
        {
            _context = context;
            _ad = ad;
            _adSettings = _ad.InitAzureSettings();
            _userObjectID = _ad.GetUserID(_context);
            _naiveCache = naiveCache;
            _authWrapper = authWrapper;
        }


        /// <summary>Acquires the authentication result.</summary>
        /// <returns></returns>
        public async Task<IAuthenticationResultWrapper> AcquireAuthenticationResult()
        {
            // To fetch the already logged in user object
            var claim = (ClaimsPrincipal)Thread.CurrentPrincipal;
            this.authContext = _authWrapper.SetAuthenticationContext(_adSettings.Authority, _naiveCache.SetUpNaiveSessionCache(userObjectID, _context.HttpContext.Session));
            ClientCredential credential = new ClientCredential(_adSettings.ClientId, _adSettings.ClientSecret);
            var result = await authContext.AcquireTokenSilentAsync(_adSettings.ProductResourceId, credential, new UserIdentifier(userObjectID, UserIdentifierType.UniqueId));
            return result;
        }


        /// <summary>Flushes the products authentication cache.</summary>
        /// <returns></returns>
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


        /// <summary>Gets the user identifier.</summary>
        /// <returns></returns>
        public UserIdentifier GetUserIdentifier()
        {
            return new UserIdentifier(this.userObjectID, UserIdentifierType.UniqueId);
        }


        /// <summary>Determines whether [is naive cached] [the specified user identifier].</summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="session">The session.</param>
        /// <returns>
        ///   <c>true</c> if [is naive cached] [the specified user identifier]; otherwise, <c>false</c>.</returns>
        public bool IsNaiveCached(string userId, ISession session)
        {
            return _naiveCache.SetUpNaiveSessionCache(userId, _context.HttpContext.Session);
        }
    }
}
