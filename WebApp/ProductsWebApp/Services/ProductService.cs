#region Namespaces
using System.Threading.Tasks;
using System.Net.Http;
using ProductListWebApp.Utils;

#endregion


/// <summary>
/// ProductListWebApp Services
/// </summary>
namespace ProductListWebApp.Services
{
    /// <summary></summary>
    /// <seealso cref="ProductListWebApp.Services.IProductService" />
    public class ProductServices : IProductService
    {
        #region PRivate Variables
        private readonly HttpClient _client;
        private readonly ISerializationHelper _serializationHelper;
        private readonly IHttpHelperService _httpHelperService;
        #endregion


        /// <summary>Initializes a new instance of the <see cref="ProductServices"/> class.</summary>
        /// <param name="client">The client.</param>
        /// <param name="serializationHelper">The serialization helper.</param>
        /// <param name="httpHelperService">The HTTP helper service.</param>
        public ProductServices(HttpClient client, ISerializationHelper serializationHelper, IHttpHelperService httpHelperService)
        {
            _client = client;
            _serializationHelper = serializationHelper;
            _httpHelperService = httpHelperService;
        }


        /// <summary>Gets the products list.</summary>
        /// <param name="AccessToken">The access token.</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> GetProductsList(string AccessToken)
        {
            var httpRequestMessage = _httpHelperService.GetHttpRequestMessageForGet(AccessToken, "/api/Product");
            return await _client.SendAsync(httpRequestMessage);
        }


        /// <summary>Creates the product.</summary>
        /// <param name="NewProduct">The new product.</param>
        /// <param name="AccessToken">The access token.</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> CreateProduct(string NewProduct, string AccessToken)
        {
            var httpRequestMessage = _httpHelperService.GetHttpRequestMessageForPost(NewProduct, AccessToken, "/api/Product");
            return await _client.SendAsync(httpRequestMessage);

        }
    }
}
