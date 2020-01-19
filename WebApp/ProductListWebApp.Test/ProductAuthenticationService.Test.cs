using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;
using ProductListWebApp.Models;
using ProductListWebApp.Services;
using ProductListWebApp.Utils;
using ProductWebApp;
using ProductWebApp.Controllers;
using ProductWebApp.Models;

namespace ProductControllerWebApp.Test
{
    [TestFixture]
    public class WhenProductAuthenticationService
    {
        private AzureActiveDirectory azureAd = new AzureActiveDirectory
        {
            ClientId = "ClientId",
            Domain = "Domain",
            Instance = "Instance",
            TenantId = "TenantId"
        };
        private Mock<IOptions<AzureActiveDirectory>> mockOptions = new Mock<IOptions<AzureActiveDirectory>>();
        HttpClient mockClient = new HttpClient();
        [SetUp]
        public void Setup()
        {
        }

        protected Mock<INaiveSessionCache> IsNaiveCached { get; private set; }
        [Test]
        public async Task NeededAuthenticationResult_Always_ReturnsStatusOK()
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

            //Assert
            var res = productAuthenticationService.AcquireAuthenticationResult();

        }
        [Test]
        public async Task RequestedAllProducts_Always_ReturnsStatusOK()
        {
            // ARRANGE
            HttpClientHelper httpClientHelper = new HttpClientHelper();
            var httpClient = httpClientHelper.GetTestHttpClient();
            Mock<IProductAuthenticationService> productAuthenticationService = new Mock<IProductAuthenticationService>();
            Mock<IProductService> productService = new Mock<IProductService>();
            Mock<ISerializationHelper> serializationHelper = new Mock<ISerializationHelper>();
            Mock<IHttpHelperService> httpHelperService = new Mock<IHttpHelperService>();
            var accessToken = "ACCESS_TOKEN";
            httpHelperService.Setup(s => s.GetHttpRequestMessageForGet(accessToken, "/api/Product")).Returns(new HttpRequestMessage()
            {
                Content = new StringContent("[{'id':1,'value':'1'}]"),
            });
            var _productServices = new ProductServices(httpClient, serializationHelper.Object, httpHelperService.Object);

            // ACT
            var result = await _productServices
               .GetProductsList(accessToken);

            // ASSERT
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

        }
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