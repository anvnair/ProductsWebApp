using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Authentication
{
    /// <summary>
    /// Settings relative to the AzureAD applications involved in this Web Application
    /// These are deserialized from the AzureAD section of the appsettings.json file
    /// </summary>
    public interface IAzureAD
    {
        AzureAdOptions InitAzureSettings();
        string GetUserID(IHttpContextAccessor Context);
    }
}
