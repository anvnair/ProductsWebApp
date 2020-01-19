namespace Microsoft.AspNetCore.Authentication
{
    /// <summary>
    /// Settings relative to the AzureAD applications involved in this Web Application
    /// These are deserialized from the AzureAD section of the appsettings.json file
    /// </summary>
    public class AzureAdOptions
    {
        private string _authority;

        /// <summary>
        /// ClientId (Application Id) of this Web Application
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Client Secret (Application password) added in the Azure portal in the Keys section for the application
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Azure AD Cloud instance
        /// </summary>
        public string Instance { get; set; }

        /// <summary>
        ///  domain of your tenant, e.g. contoso.onmicrosoft.com
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Tenant Id, as obtained from the Azure portal:
        /// (Select 'Endpoints' from the 'App registrations' blade and use the GUID in any of the URLs)
        /// </summary>
        public string TenantId { get; set; }

        /// <summary>
        /// URL on which this Web App will be called back by Azure AD (normally "/signin-oidc")
        /// </summary>
        public string CallbackPath { get; set; }

        /// <summary>
        /// Authority delivering the token for your tenant
        /// </summary>
        public string Authority
        {
            get
            {
                return $"{Instance}{TenantId}" == string.Empty ? "https://login.microsoftonline.com/a58751a8-7872-489f-886f-72392719889e" : $"{Instance}{TenantId}";
            }
            set
            {
                _authority = value;
            }
        }

        /// <summary>
        /// Client Id (Application ID) of the ProductService, obtained from the Azure portal for that application
        /// </summary>
        public string ProductResourceId { get; set; }

        /// <summary>
        /// Base URL of the ProductService
        /// </summary>
        public string ProductBaseAddress { get; set; }

        /// <summary>
        /// Instance of the settings for this Web application (to be used in controllers)
        /// </summary>
        public static AzureAdOptions Settings { set; get; }
    }
}
