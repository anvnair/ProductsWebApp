#region Namespaces
using System.Net.Http;

#endregion

/// <summary>
/// Product List WebApp Services
/// </summary>
namespace ProductListWebApp.Services
{
    /// <summary>Http Helper Service</summary>
    public interface IHttpHelperService
    {
        HttpRequestMessage GetHttpRequestMessageForGet(string AccessToken, string Url);
        HttpRequestMessage GetHttpRequestMessageForPost(string NewProduct, string AccessToken, string url);
    }
}
