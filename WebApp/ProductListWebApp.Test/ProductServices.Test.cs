#region Namespaces
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using ProductControllerWebApp.Test;
using ProductListWebApp.Models;
using ProductListWebApp.Services;
using ProductListWebApp.Utils;
using ProductWebApp.Models;
#endregion

/// <summary>
/// Product Test
/// </summary>
namespace ProductsApp.Test
{
    /// Product Service Test <summary>
    ///   <para></para>
    ///   <para></para>
    /// </summary>
    [TestFixture]
    public class WhenProductsService
    {
        #region Local Declarations
        private Mock<IOptions<AzureActiveDirectory>> mockOptions = new Mock<IOptions<AzureActiveDirectory>>();
        HttpClient mockClient = new HttpClient();
        #endregion

        /// <summary>Addeds the products always returns status ok.</summary>
        [Test]
        public async Task AddedProducts_Always_AddSuccessfully()
        {
            // ARRANGE
            HttpClientHelper httpClientHelper = new HttpClientHelper();
            var httpClient = httpClientHelper.GetTestHttpClient();
            Mock<IProductAuthenticationService> productAuthenticationService = new Mock<IProductAuthenticationService>();
            Mock<IProductService> productService = new Mock<IProductService>();
            Mock<ISerializationHelper> serializationHelper = new Mock<ISerializationHelper>();
            Mock<IHttpHelperService> httpHelperService = new Mock<IHttpHelperService>();
            var accessToken = PRODUCTS_TEST_CONSTANTS.ACCESS_TOKEN;
            var newProduct = PRODUCTS_TEST_CONSTANTS.NEW_PRODUCT;
            httpHelperService.Setup(s => s.GetHttpRequestMessageForPost(newProduct, accessToken, PRODUCTS_TEST_CONSTANTS.API_URL)).Returns(new HttpRequestMessage()
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

        /// <summary>Requesteds all products always returns status ok.</summary>
        [Test]
        public async Task RequestedAllProducts_Always_ReturnsProducts()
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
    }
}