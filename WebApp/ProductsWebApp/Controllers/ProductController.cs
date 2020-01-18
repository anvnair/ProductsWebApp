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
using ProductListWebApp.Services;
using ProductListWebApp.Utils;
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
        public AuthenticationResult result { get; set; }
        IProductAuthenticationService _authenticationService;
        IProductService _productService;
        ISerializationHelper _serializationHelper;
        public ProductController(IProductAuthenticationService authenticationService, IProductService ProductService, ISerializationHelper serializationHelper)
        {
            _authenticationService = authenticationService;
            _productService = ProductService;
            _serializationHelper = serializationHelper;
        }
        // GET: /<controller>/
        /// <summary>Indexes this instance.</summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            IAuthenticationResultWrapper authenticationResult = null;
            List<ProductItem> products = new List<ProductItem>();

            try
            {
                authenticationResult = await _authenticationService.AcquireAuthenticationResult();
                HttpResponseMessage response = await _productService.GetProductsList(authenticationResult.AccessToken);

                // Return the Product List in the view.
                if (response.IsSuccessStatusCode)
                {
                    var _productList = _serializationHelper.Deserialize(await response.Content.ReadAsStringAsync());
                    return View(_productList);
                }

                //
                // If the call failed with access denied, then drop the current access token from the cache, 
                //     and show the user an error indicating they might need to sign-in again.
                //
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return ProcessUnauthorized();
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
                products.Add(newItem);
                ViewBag.ErrorMessage = "AuthorizationRequired";
                return View(products);
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

                IAuthenticationResultWrapper authenticationResult = null;
                List<ProductItem> itemList = new List<ProductItem>();

                try
                {
                    authenticationResult = await _authenticationService.AcquireAuthenticationResult();

                    HttpResponseMessage createProductResponse = await _productService.CreateProduct(_serializationHelper.ConvertObjectToJSON(item), authenticationResult.AccessToken);

                    // Return the Product List in the view.                    
                    if (createProductResponse.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }

                    //
                    // If the call failed with access denied, then drop the current access token from the cache, 
                    //     and show the user an error indicating they might need to sign-in again.
                    //
                    if (createProductResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        return ProcessUnauthorized();
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
        private ActionResult ProcessUnauthorized()
        {
            List<ProductItem> itemList = new List<ProductItem>();
            if (_authenticationService.FlushProductsAuthenticationCache())
            {
                ViewBag.ErrorMessage = "UnexpectedError";
                ProductItem newItem = new ProductItem();
                newItem.Title = "(No items in list)";
                itemList.Add(newItem);
            }
            return View(itemList);
        }
    }
}
