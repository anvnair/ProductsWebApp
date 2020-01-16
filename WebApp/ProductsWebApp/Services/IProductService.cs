using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProductListWebApp.Services
{
    public interface IProductService
    {
        Task<HttpResponseMessage> GetProductsList(string AccessToken);
    }
}
