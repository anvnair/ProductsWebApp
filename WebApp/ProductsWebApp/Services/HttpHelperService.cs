#region Namespaces
using System;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http;
using System.Net.Http.Headers;

#endregion


/// <summary>
/// ProductList WebApp Services
/// </summary>
namespace ProductListWebApp.Services
{
    /// <summary></summary>
    /// <seealso cref="ProductListWebApp.Services.IHttpHelperService" />
    public class HttpHelperService : IHttpHelperService
    {
        public HttpRequestMessage GetHttpRequestMessageForGet(string AccessToken, string Url)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, AzureAdOptions.Settings.ProductBaseAddress + Url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
                return request;
            }
            catch (Exception)
            {
                return new HttpRequestMessage {};
            }
        }


        /// <summary>Gets the HTTP request message for post.</summary>
        /// <param name="NewProduct">The new product.</param>
        /// <param name="AccessToken">The access token.</param>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public HttpRequestMessage GetHttpRequestMessageForPost(string NewProduct, string AccessToken, string url)
        {
            try
            {
                HttpContent requestContentProduct = new StringContent(NewProduct, System.Text.Encoding.UTF8, "application/json");
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, AzureAdOptions.Settings.ProductBaseAddress + url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
                request.Content = requestContentProduct;
                return request;
            }
            catch (Exception)
            {
                return new HttpRequestMessage { };
            }
        }

    }
}
