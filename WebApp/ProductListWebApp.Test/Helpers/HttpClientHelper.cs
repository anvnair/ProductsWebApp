#region Namespaces
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;

#endregion
/// <summary>
/// ProductControllerWebApp Test
/// </summary>
namespace ProductControllerWebApp.Test
{
    /// <summary></summary>
    public class HttpClientHelper
    {
        public HttpClient GetTestHttpClient()
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               // Setup the PROTECTED method to mock
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               // prepare the expected response of the mocked http call
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent("[{'id':1,'value':'1'}]"),
               })
               .Verifiable();

            // use real http client with mocked handler here
            return new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://localhost:44351"),
            };
        }
    }
}