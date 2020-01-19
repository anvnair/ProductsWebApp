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
using ProductWebApp.Controllers;
using ProductWebApp.Models;

namespace ProductControllerWebApp.Test
{
    [TestFixture]
    public class WhenProductsService
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

        [Test]
        public async Task AddedProducts_Always_ReturnsStatusOK()
        {
            // ARRANGE
            HttpClientHelper httpClientHelper = new HttpClientHelper();
            var httpClient = httpClientHelper.GetTestHttpClient();
            Mock<IProductAuthenticationService> productAuthenticationService = new Mock<IProductAuthenticationService>();
            Mock<IProductService> productService = new Mock<IProductService>();
            Mock<ISerializationHelper> serializationHelper = new Mock<ISerializationHelper>();
            Mock<IHttpHelperService> httpHelperService = new Mock<IHttpHelperService>();
            var accessToken = "ACCESS_TOKEN";
            var newProduct = "ACCESS_TOKEN";
            httpHelperService.Setup(s => s.GetHttpRequestMessageForPost(newProduct, accessToken, "/api/Product")).Returns(new HttpRequestMessage()
            {
                Content = new StringContent("[{'id':1,'value':'1'}]"),
            });
            var _productServices = new ProductServices(httpClient, serializationHelper.Object, httpHelperService.Object);

            // ACT
            var result = await _productServices
               .CreateProduct(newProduct, accessToken);

            // ASSERT
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
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

            //result.Should().NotBeNull(); // this is fluent assertions here...
            //result.Id.Should().Be(1);

            //// also check the 'http' call was like we expected it
            //var expectedUri = new Uri("http://test.com/api/test/whatever");

            //handlerMock.Protected().Verify(
            //   "SendAsync",
            //   Times.Exactly(1), // we expected a single external request
            //   ItExpr.Is<HttpRequestMessage>(req =>
            //      req.Method == HttpMethod.Get  // we expected a GET request
            //      && req.RequestUri == expectedUri // to this uri
            //   ),
            //   ItExpr.IsAny<CancellationToken>()
            //);
        }
        //[Test]
        //public async Task Get_WhenAllUsers_ReturnsAllUsers()
        //{
        //    //Arrange
        //    // AuthenticationResultWrapper _authResult = new AuthenticationResultWrapper();
        //    Mock<IProductAuthenticationService> productAuthenticationService = new Mock<IProductAuthenticationService>();
        //    Mock<IProductService> productService = new Mock<IProductService>();
        //    Mock<ISerializationHelper> serializationHelper = new Mock<ISerializationHelper>();
        //    Mock<HttpResponse> httpResponse = new Mock<HttpResponse>();
        //    var authResult = new Mock<IAuthenticationResultWrapper>();
        //    var authContext = new Mock<IAuthenticationContextWrapper>();
        //    //var wrapper = new AdalWrapper(this.AuthenticationContext.Object);

        //    var resourceUrl = "http://localhost";
        //    var clientId = "CLIENT_ID";
        //    var clientSecret = "CLIENT_SECRET";
        //    var accessToken = "ACCESS_TOKEN";

        //    authResult.SetupGet(p => p.AccessToken).Returns(accessToken);
        //    authContext.Setup(p => p.AcquireTokenAsync(It.IsAny<string>(), It.IsAny<ClientCredential>())).ReturnsAsync(authResult.Object);
        //    productAuthenticationService.Setup(pas => pas.AcquireAuthenticationResult()).ReturnsAsync(authResult.Object);
        //    //productService.Setup(ps => ps.GetProductsList(accessToken)).ReturnsAsync(httpResponse.Object);


        //    //Act
        //    var productController = new ProductController(productAuthenticationService.Object, productService.Object, serializationHelper.Object);
        //    var result = await productController.Index();

        //    //Assert


        //    //HttpContent content = new StringContent(JsonConvert.SerializeObject(GetAllProductItem()), Encoding.UTF8, "application/json");
        //    //mockOptions.Setup(p => p.Value).Returns(azureAd);
        //    //var controller = new ProductController();
        //    //var actionResult = await controller.Index() as Task<IActionResult>;
        //    //Assert.IsInstanceOf<ViewResult>(actionResult);
        //}
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