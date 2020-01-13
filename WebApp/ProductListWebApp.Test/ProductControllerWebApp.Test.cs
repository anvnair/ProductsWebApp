using System.Collections.Generic;
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
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using ProductListWebApp.Models;
using ProductWebApp.Controllers;
using ProductWebApp.Models;

namespace Tests
{
    public class Tests
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
        public async Task IndexTestAsync()
        { 
            HttpContent content = new StringContent(JsonConvert.SerializeObject(GetAllProductItem()), Encoding.UTF8, "application/json");
            mockOptions.Setup(p => p.Value).Returns(azureAd);
            var controller = new ProductController();
            var actionResult = await controller.Index() as Task<IActionResult>;
            Assert.IsInstanceOf<ViewResult>(actionResult);
        }
        private List<ProductItem> GetAllProductItem()
        {
            var productItemList = new List<ProductItem> {
                 new ProductItem { Title = "first ProductItem" },
                 new ProductItem { Title = "second ProductItem" }
               };
            return productItemList;

        }
    }
}