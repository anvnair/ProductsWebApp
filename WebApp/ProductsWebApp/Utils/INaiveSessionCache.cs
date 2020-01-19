#region Namespaces
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
#endregion

/// <summary>
/// Product app Naice Session
/// </summary>
namespace ProductWebApp
{
    /// <summary></summary>
    public interface INaiveSessionCache 
    {

        /// <summary>Sets up naive session cache.</summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="session">The session.</param>
        /// <returns></returns>
        bool SetUpNaiveSessionCache(string userId, ISession session);


        /// <summary>Loads this instance.</summary>
        /// <returns></returns>
        bool Load();


        /// <summary>Persists this instance.</summary>
        void Persist();

        // Triggered right before ADAL needs to access the cache.
        // Reload the cache from the persistent store in case it changed since the last access.
        void BeforeAccessNotification(TokenCacheNotificationArgs args);

        // Triggered right after ADAL accessed the cache.
        void AfterAccessNotification(TokenCacheNotificationArgs args);
    }
}
