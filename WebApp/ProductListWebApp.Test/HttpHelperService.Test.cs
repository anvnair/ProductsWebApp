#region Namespaces
using System.Net.Http;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using ProductListWebApp.Models;
using ProductListWebApp.Services;
#endregion

/// <summary>
/// Tests for Products app
/// </summary>
namespace ProductsApp.Test
{
    /// <summary>When Http Helper Service Called For</summary>
    [TestFixture]
    public class WhenHttpHelperServiceCalledFor
    {
        #region Declarations
        private Mock<IOptions<AzureActiveDirectory>> mockOptions = new Mock<IOptions<AzureActiveDirectory>>();
        HttpClient mockClient = new HttpClient();
        #endregion

        /// <summary>HTTPs the request message for get always returns not null.</summary>
        [Test]
        public void HttpRequestMessageForGet_Always_ReturnsNotNull()
        {
            //Arrange
            var url = "URL";
            var accessToken = "ACCESS_TOKEN";
            Mock<IHttpHelperService> httpHelperService = new Mock<IHttpHelperService>();
            httpHelperService.Setup(h => h.GetHttpRequestMessageForGet(accessToken, url)).Returns(new HttpRequestMessage() { });
            var http = new HttpHelperService();

            //Act
            var result = http.GetHttpRequestMessageForGet(accessToken, url);

            //Assert
            Assert.IsNotNull(result.Version);
        }

        /// <summary>HTTPs the request message for post always returns not null.</summary>
        [Test]
        public void HttpRequestMessageForPost_Always_ReturnsNotNull()
        {
            //Arrange
            var url = "URL";
            var accessToken = "ACCESS_TOKEN";
            var newProduct = "New_Product";
            Mock<IHttpHelperService> httpHelperService = new Mock<IHttpHelperService>();
            httpHelperService.Setup(h => h.GetHttpRequestMessageForPost(newProduct, accessToken, url)).Returns(new HttpRequestMessage() { });
            var http = new HttpHelperService();

            //Act
            var result = http.GetHttpRequestMessageForGet(accessToken, url);

            //Assert
            Assert.IsNotNull(result.Version);
        }
    }
}