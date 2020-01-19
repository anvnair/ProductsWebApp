#region Namespaeces
using Microsoft.AspNetCore.Http;
#endregion


/// <summary>
/// Namespace for Microsoft.AspNetCore.Authentication
/// </summary>
namespace Microsoft.AspNetCore.Authentication
{
    /// <summary>Settings relative to the AzureAD applications involved in this Web Application
    /// These are deserialized from the AzureAD section of the appsettings.json file</summary>
    public class AzureAD : IAzureAD
    {
        /// <summary>
        ///   <para></para>
        ///   <para>The settings
        /// </para>
        /// </summary>
        public AzureAdOptions _settings;

        /// <summary>Gets or sets the settings.</summary>
        /// <value>The settings.</value>
        public AzureAdOptions Settings { get; set; }

        /// <summary>Initializes the azure settings.</summary>
        /// <returns></returns>
        public AzureAdOptions InitAzureSettings()
        {
            return AzureAdOptions.Settings;
        }
        /// <summary>Gets the user identifier.</summary>
        /// <param name="Context">The context.</param>
        /// <returns></returns>
        public string GetUserID(IHttpContextAccessor Context)
        {
            return Context?.HttpContext?.User != null ? (Context.HttpContext.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier"))?.Value : "1";
        }
    }
}
