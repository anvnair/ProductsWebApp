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
    public class WhenHttpHelperServiceCalledFor
    {

        private Mock<IOptions<AzureActiveDirectory>> mockOptions = new Mock<IOptions<AzureActiveDirectory>>();
        HttpClient mockClient = new HttpClient();
        [SetUp]
        public void Setup()
        {
        }
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