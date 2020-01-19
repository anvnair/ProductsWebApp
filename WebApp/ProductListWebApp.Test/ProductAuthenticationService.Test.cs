#region Namespaces
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Moq;
using NUnit.Framework;
using ProductControllerWebApp.Test;
using ProductListWebApp.Models;
using ProductListWebApp.Services;
using ProductListWebApp.Utils;
using ProductWebApp;
using ProductWebApp.Models;
#endregion

/// <summary>
/// Product Tests
/// </summary>
namespace ProductsApp.Test
{
    [TestFixture]
    public class WhenProductAuthenticationService
    {
        #region Local Declarations
        private Mock<IOptions<AzureActiveDirectory>> mockOptions = new Mock<IOptions<AzureActiveDirectory>>();
        HttpClient mockClient = new HttpClient();
        protected Mock<INaiveSessionCache> IsNaiveCached { get; private set; }
        #endregion

        /// <summary>Neededs the authentication result always returns status ok.</summary>
        [Test]
        public async Task NeededAuthenticationResult_Always_AuthenticateSuccessfully()
        {
            // ARRANGE
            Mock<IProductAuthenticationService> productAuthenticationServiceMock = new Mock<IProductAuthenticationService>();
            Mock<IHttpContextAccessor> httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            Mock<INaiveSessionCache> _naiveCacheMock = new Mock<INaiveSessionCache>();
            Mock<IAuthenticationContextWrapper> authContextMock = new Mock<IAuthenticationContextWrapper>();
            Mock<IAuthenticationResultWrapper> authResult = new Mock<IAuthenticationResultWrapper>();

            //Mock Claims Principal
            IList<Claim> claimCollection = new List<Claim>
                {
                    new Claim("name", "John Doe")
                };
            var identityMock = new Mock<ClaimsIdentity>();
            identityMock.Setup(x => x.Claims).Returns(claimCollection);
            var cp = new Mock<ClaimsPrincipal>();
            cp.Setup(m => m.HasClaim(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            cp.Setup(m => m.Identity).Returns(identityMock.Object);
            Thread.CurrentPrincipal = cp.Object;

            // Ihttp Context mock
            var context = new DefaultHttpContext();
            context.Session = new MockHttpSession();
            httpContextAccessorMock.SetupGet(p => p.HttpContext).Returns(context);

            //IAzure AD
            Mock<IAzureAD> _azureAD = new Mock<IAzureAD>();
            _azureAD.Setup(a => a.GetUserID(httpContextAccessorMock.Object)).Returns("8999ffbe-8d75-4f86-b499-179f7728ae44");
            _azureAD.Setup(ad => ad.InitAzureSettings()).Returns(new AzureAdOptions
            {
                Authority = "https://login.microsoftonline.com/a58751a8-7872-489f-886f-72392719889e",
                ClientId = "ClientId",
                ClientSecret = "ClientSecret",
                ProductResourceId = "ProductResourceId"
            });

            //INaive Cache
            _naiveCacheMock.Setup(n => n.SetUpNaiveSessionCache("8999ffbe-8d75-4f86-b499-179f7728ae44", context.Session)).Returns(true);

            //Iauthentication context mock            
            authContextMock.Setup(p => p.AcquireTokenAsync(It.IsAny<string>(), It.IsAny<ClientCredential>())).ReturnsAsync(authResult.Object);
            authContextMock.Setup(a => a.SetAuthenticationContext("https://login.microsoftonline.com/a58751a8-7872-489f-886f-72392719889e", true)).Returns(new AuthenticationContextWrapper("https://login.microsoftonline.com/a58751a8-7872-489f-886f-72392719889e", true) { });
            authContextMock.Setup(p => p.AcquireTokenSilentAsync(It.IsAny<string>(), It.IsAny<ClientCredential>(), productAuthenticationServiceMock.Object.GetUserIdentifier())).ReturnsAsync(authResult.Object);

            //IAuth Result
            var accessToken = "ACCESS_TOKEN";
            authResult.SetupGet(p => p.AccessToken).Returns(accessToken);

            productAuthenticationServiceMock.Setup(s => s.IsNaiveCached("user", context.Session)).Returns(true);
            // productAuthenticationServiceMock.Setup(s => s.GetUserIdentifier()).Returns(new UserIdentifier("id", UserIdentifierType.UniqueId) { });
            productAuthenticationServiceMock.Setup(s => s.AcquireAuthenticationResult()).ReturnsAsync(authResult.Object);

            //Act
            var productAuthenticationService = new ProductAuthenticationService(httpContextAccessorMock.Object, _azureAD.Object, _naiveCacheMock.Object, authContextMock.Object);

            //Assert for not null
            Assert.IsNotNull(productAuthenticationService._adSettings);
             
        }

        /// <summary>Requesteds all products always returns status ok.</summary>
        [Test]
        public async Task RequestedFlushCache_Always_FlushAllCachedItems()
        {
            // ARRANGE
            Mock<IProductAuthenticationService> productAuthenticationServiceMock = new Mock<IProductAuthenticationService>();
            Mock<IHttpContextAccessor> httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            Mock<INaiveSessionCache> _naiveCacheMock = new Mock<INaiveSessionCache>();
            Mock<IAuthenticationContextWrapper> authContextMock = new Mock<IAuthenticationContextWrapper>();
            Mock<IAuthenticationResultWrapper> authResult = new Mock<IAuthenticationResultWrapper>();

            //Mock Claims Principal
            IList<Claim> claimCollection = new List<Claim>
                {
                    new Claim("name", "John Doe")
                };
            var identityMock = new Mock<ClaimsIdentity>();
            identityMock.Setup(x => x.Claims).Returns(claimCollection);
            var cp = new Mock<ClaimsPrincipal>();
            cp.Setup(m => m.HasClaim(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            cp.Setup(m => m.Identity).Returns(identityMock.Object);
            Thread.CurrentPrincipal = cp.Object;

            // Ihttp Context mock
            var context = new DefaultHttpContext();
            context.Session = new MockHttpSession();
            httpContextAccessorMock.SetupGet(p => p.HttpContext).Returns(context);

            //IAzure AD
            Mock<IAzureAD> _azureAD = new Mock<IAzureAD>();
            _azureAD.Setup(a => a.GetUserID(httpContextAccessorMock.Object)).Returns("8999ffbe-8d75-4f86-b499-179f7728ae44");
            _azureAD.Setup(ad => ad.InitAzureSettings()).Returns(new AzureAdOptions
            {
                Authority = "https://login.microsoftonline.com/a58751a8-7872-489f-886f-72392719889e",
                ClientId = "ClientId",
                ClientSecret = "ClientSecret",
                ProductResourceId = "ProductResourceId"
            });

            //INaive Cache
            _naiveCacheMock.Setup(n => n.SetUpNaiveSessionCache("8999ffbe-8d75-4f86-b499-179f7728ae44", context.Session)).Returns(true);

            //Iauthentication context mock            
            authContextMock.Setup(p => p.AcquireTokenAsync(It.IsAny<string>(), It.IsAny<ClientCredential>())).ReturnsAsync(authResult.Object);
            authContextMock.Setup(a => a.SetAuthenticationContext("https://login.microsoftonline.com/a58751a8-7872-489f-886f-72392719889e", true)).Returns(new AuthenticationContextWrapper("https://login.microsoftonline.com/a58751a8-7872-489f-886f-72392719889e", true) { });
            authContextMock.Setup(p => p.AcquireTokenSilentAsync(It.IsAny<string>(), It.IsAny<ClientCredential>(), productAuthenticationServiceMock.Object.GetUserIdentifier())).ReturnsAsync(authResult.Object);

            //IAuth Result
            var accessToken = "ACCESS_TOKEN";
            authResult.SetupGet(p => p.AccessToken).Returns(accessToken);

            productAuthenticationServiceMock.Setup(s => s.IsNaiveCached("user", context.Session)).Returns(true);
            // productAuthenticationServiceMock.Setup(s => s.GetUserIdentifier()).Returns(new UserIdentifier("id", UserIdentifierType.UniqueId) { });
            productAuthenticationServiceMock.Setup(s => s.AcquireAuthenticationResult()).ReturnsAsync(authResult.Object);

            //Act
            var productAuthenticationService = new ProductAuthenticationService(httpContextAccessorMock.Object, _azureAD.Object, _naiveCacheMock.Object, authContextMock.Object); 

            //Assert for cache
            Assert.IsFalse(productAuthenticationService.FlushProductsAuthenticationCache());
        }

        /// <summary>Gets all product item.</summary>
        /// <returns></returns>
        private List<ProductItemViewModel> GetAllProductItem()
        {
            var productItemList = new List<ProductItemViewModel> {
                 new ProductItemViewModel { Title = "first ProductItem" },
                 new ProductItemViewModel { Title = "second ProductItem" }
               };
            return productItemList;

        }
    }
}