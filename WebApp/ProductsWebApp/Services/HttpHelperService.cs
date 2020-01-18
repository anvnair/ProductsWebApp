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

namespace ProductListWebApp.Services
{
    public class HttpHelperService : IHttpHelperService
    {
        public HttpRequestMessage GetHttpRequestMessageForGet(string AccessToken, string Url)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, AzureAdOptions.Settings.ProductBaseAddress + Url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            return request;
        }

        public HttpRequestMessage GetHttpRequestMessageForPost(string NewProduct, string AccessToken, string url)
        {
            HttpContent requestContentProduct = new StringContent(NewProduct, System.Text.Encoding.UTF8, "application/json");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, AzureAdOptions.Settings.ProductBaseAddress + url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            request.Content = requestContentProduct;
            return request;
        }

    }
}
