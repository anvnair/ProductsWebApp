using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Authentication
{
    /// <summary>
    /// Settings relative to the AzureAD applications involved in this Web Application
    /// These are deserialized from the AzureAD section of the appsettings.json file
    /// </summary>
    public class AzureAD : IAzureAD
    {
        private string _authority, _clientId;
        public AzureAdOptions _settings;
        public AzureAdOptions Settings { get; set; }
        public AzureAdOptions InitAzureSettings()
        {
            return AzureAdOptions.Settings;
        }

        public string GetUserID(IHttpContextAccessor Context)
        {
            return Context?.HttpContext?.User != null ? (Context.HttpContext.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier"))?.Value : "1";
        }
    }
}
