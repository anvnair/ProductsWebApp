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
            var httpRequestMessage = _httpHelperService.GetHttpRequestMessageForGet(AccessToken, "/api/Product");
            return await _client.SendAsync(httpRequestMessage);
        }

        public async Task<HttpResponseMessage> CreateProduct(string NewProduct, string AccessToken)
        {
            var httpRequestMessage = _httpHelperService.GetHttpRequestMessageForPost(NewProduct, AccessToken, "/api/Product");
            return await _client.SendAsync(httpRequestMessage);

        }
    }
}
