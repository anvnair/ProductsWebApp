#region Namespaces
using System.Net.Http;
using System.Threading.Tasks;
#endregion


/// <summary>
///  Product List Web App Services
/// </summary>
namespace ProductListWebApp.Services
{
    /// <summary>Product Service</summary>
    public interface IProductService
    {
        Task<HttpResponseMessage> GetProductsList(string AccessToken);
        Task<HttpResponseMessage> CreateProduct(string NewProduct, string AccessToken);
    }
}
