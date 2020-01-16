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
using System.Net.Http;
using System.Net.Http.Headers;

namespace ProductListWebApp.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _client;
        public ProductService(HttpClient client)
        {
            _client = client;
        }

        public async Task<HttpResponseMessage> GetProductsList(string AccessToken)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, AzureAdOptions.Settings.ProductBaseAddress + "/api/Product");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            return await _client.SendAsync(request);
        }
    }
}
