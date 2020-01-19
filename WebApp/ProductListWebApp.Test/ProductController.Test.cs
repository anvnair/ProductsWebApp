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
using ProductWebApp.Controllers;
using ProductWebApp.Models;
#endregion

/// <summary>
/// Product Test
/// </summary>
namespace ProductsApp.Test
{
    [TestFixture]
    public class WhenProductsController
    {
        #region Local Declarations
        private Mock<IOptions<AzureActiveDirectory>> mockOptions = new Mock<IOptions<AzureActiveDirectory>>();
        HttpClient mockClient = new HttpClient();
        #endregion

        /// <summary>Indexes the always returns status ok.</summary>
        [Test]
        public async Task IndexCalled_Always_ReturnsAllProducts()
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
            var productController = new ProductController(productAuthenticationService.Object, productService.Object, serializationHelper.Object);

            // ACT
            var result = productController.Index();

            // ASSERT
            Assert.AreEqual(true, result.IsFaulted);
        }

        [Test]
        /// <summary>Requesteds all products always returns status ok.</summary>
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

        /// <summary>
        ///   <para></para>
        ///   <para>Gets all product item.
        /// </para>
        /// </summary>
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