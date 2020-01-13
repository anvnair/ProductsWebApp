#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using ProductWebApp.Models;
#endregion


/// <summary>
/// Main controller for Product app, act s entry point
/// </summary>
namespace ProductWebApp.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        // GET: /<controller>/
        /// <summary>Indexes this instance.</summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            AuthenticationResult result = null;
            List<ProductItem> itemList = new List<ProductItem>();

            try
            {
                // To fetch the already logged in user object
                string userObjectID = User != null ? (User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier"))?.Value : "1";

                // Using ADAL.Net, get a bearer token to access the ProductService
                AuthenticationContext authContext = new AuthenticationContext(AzureAdOptions.Settings.Authority, new NaiveSessionCache(userObjectID, HttpContext.Session));
                ClientCredential credential = new ClientCredential(AzureAdOptions.Settings.ClientId, AzureAdOptions.Settings.ClientSecret);
                result = await authContext.AcquireTokenSilentAsync(AzureAdOptions.Settings.ProductResourceId, credential, new UserIdentifier(userObjectID, UserIdentifierType.UniqueId));

                // Retrieve the user's Product List.
                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, AzureAdOptions.Settings.ProductBaseAddress + "/api/Product");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
                HttpResponseMessage response = await client.SendAsync(request);

                // Return the Product List in the view.
                if (response.IsSuccessStatusCode)
                {
                    List<Dictionary<String, String>> responseElements = new List<Dictionary<String, String>>();
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    String responseString = await response.Content.ReadAsStringAsync();
                    responseElements = JsonConvert.DeserializeObject<List<Dictionary<String, String>>>(responseString, settings);
                    foreach (Dictionary<String, String> responseElement in responseElements)
                    {
                        ProductItem newItem = new ProductItem();
                        newItem.Title = responseElement["title"];
                        newItem.Owner = responseElement["owner"];
                        itemList.Add(newItem);
                    }

                    return View(itemList);
                }

                //
                // If the call failed with access denied, then drop the current access token from the cache, 
                //     and show the user an error indicating they might need to sign-in again.
                //
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return ProcessUnauthorized(itemList, authContext);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                if (HttpContext.Request.Query["reauth"] == "True")
                {
                    //
                    // Send an OpenID Connect sign-in request to get a new set of tokens.
                    // If the user still has a valid session with Azure AD, they will not be prompted for their credentials.
                    // The OpenID Connect middleware will return to this controller after the sign-in response has been handled.
                    //
                    return new ChallengeResult(OpenIdConnectDefaults.AuthenticationScheme);
                }

                //
                // The user needs to re-authorize.  Show them a message to that effect.
                //
                ProductItem newItem = new ProductItem();
                newItem.Title = "(Sign-in required to view Product list.)";
                itemList.Add(newItem);
                ViewBag.ErrorMessage = "AuthorizationRequired";
                return View(itemList);
            }

            //
            // If the call failed for any other reason, show the user an error.
            //
            return View("Error");
        }

        [HttpPost]
        public async Task<ActionResult> Index(string item)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the user's tenantID and access token since they are parameters used to call the Product service.

                AuthenticationResult result = null;
                List<ProductItem> itemList = new List<ProductItem>();

                try
                {
                    string userObjectID = (User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier"))?.Value;
                    AuthenticationContext authContext = new AuthenticationContext(AzureAdOptions.Settings.Authority, new NaiveSessionCache(userObjectID, HttpContext.Session));
                    ClientCredential credential = new ClientCredential(AzureAdOptions.Settings.ClientId, AzureAdOptions.Settings.ClientSecret);
                    result = await authContext.AcquireTokenSilentAsync(AzureAdOptions.Settings.ProductResourceId, credential, new UserIdentifier(userObjectID, UserIdentifierType.UniqueId));

                    // Forms encode Product item, to POST to the Product list web api.
                    HttpContent content = new StringContent(JsonConvert.SerializeObject(new { Title = item }), System.Text.Encoding.UTF8, "application/json");

                    // Add the item to user's Product List.
                    //
                    HttpClient client = new HttpClient();
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, AzureAdOptions.Settings.ProductBaseAddress + "/api/Product");
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
                    request.Content = content;
                    HttpResponseMessage response = await client.SendAsync(request);


                    // Return the Product List in the view.
                    //
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }

                    //
                    // If the call failed with access denied, then drop the current access token from the cache, 
                    //     and show the user an error indicating they might need to sign-in again.
                    //
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        return ProcessUnauthorized(itemList, authContext);
                    }
                }
                catch (Exception)
                {
                    //
                    // The user needs to re-authorize.  Show them a message to that effect.
                    //
                    ProductItem newItem = new ProductItem();
                    newItem.Title = "(No items in list)";
                    itemList.Add(newItem);
                    ViewBag.ErrorMessage = "AuthorizationRequired";
                    return View(itemList);
                }
                //
                // If the call failed for any other reason, show the user an error.
                //
                return View("Error");
            }
            return View("Error");
        }

        /// <summary>Processes the unauthorized.</summary>
        /// <param name="itemList">The item list.</param>
        /// <param name="authContext">The authentication context.</param>
        /// <returns></returns>
        private ActionResult ProcessUnauthorized(List<ProductItem> itemList, AuthenticationContext authContext)
        {
            var ProductTokens = authContext.TokenCache.ReadItems().Where(a => a.Resource == AzureAdOptions.Settings.ProductResourceId);
            foreach (TokenCacheItem tci in ProductTokens)
                authContext.TokenCache.DeleteItem(tci);

            ViewBag.ErrorMessage = "UnexpectedError";
            ProductItem newItem = new ProductItem();
            newItem.Title = "(No items in list)";
            itemList.Add(newItem);
            return View(itemList);
        }
    }
}
