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
using ProductListWebApp.Utils;
using ProductWebApp.Models;

namespace ProductListWebApp.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _client;
        private readonly ISerializationHelper _serializationHelper;
        private readonly IHttpHelperService _httpHelperService;

        public ProductService(HttpClient client, ISerializationHelper serializationHelper, IHttpHelperService httpHelperService)
        {
            _client = client;
            _serializationHelper = serializationHelper;
            _httpHelperService = httpHelperService;
        }

        public async Task<HttpResponseMessage> GetProductsList(string AccessToken)
        {
            return await _client.SendAsync(_httpHelperService.GetHttpRequestMessageForGet(AccessToken, "/api/Product"));
        }
        //public async Task<HttpResponseMessage> CreateProduct(string NewProduct, string AccessToken)
        //{
        //    return await _client.SendAsync(_httpHelperService.GetHttpRequestMessageForPost(NewProduct, AccessToken, "api / Product"));
        //}
        public async Task<HttpResponseMessage> CreateProduct(string NewProduct, string AccessToken)
        {
            HttpContent requestContentProduct = new StringContent(NewProduct, System.Text.Encoding.UTF8, "application/json");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, AzureAdOptions.Settings.ProductBaseAddress + "/api/Product");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            request.Content = requestContentProduct;
            return await _client.SendAsync(request);
        }
    }
}
