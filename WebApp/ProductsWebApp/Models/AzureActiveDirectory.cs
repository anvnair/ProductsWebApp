#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#endregion

/// <summary>
/// Names space for ProductListWebApp.Models
/// </summary>
namespace ProductListWebApp.Models
{
    /// <summary>Azure Active Directory</summary>
    public class AzureActiveDirectory
    {
        public string Instance { get; set; }
        public string Domain { get; set; }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string ResourceId { get; set; }
        public string CallbackPath { get; set; }
        public string ApiUrl { get; set; }
    }
}

