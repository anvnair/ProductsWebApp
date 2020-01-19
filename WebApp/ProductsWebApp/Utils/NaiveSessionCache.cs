using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace ProductWebApp
{
    public class NaiveSessionCache : TokenCache, INaiveSessionCache
    {
        private static readonly object FileLock = new object();
        string UserObjectId = string.Empty;
        string CacheId = string.Empty;
        ISession Session = null;

        public bool SetUpNaiveSessionCache(string userId, ISession session)
        {
            UserObjectId = userId;
            CacheId = UserObjectId + "_TokenCache";
            Session = session;
            this.AfterAccess = AfterAccessNotification;
            this.BeforeAccess = BeforeAccessNotification;
            return Load();

        }


        public bool Load()
        {
            lock (FileLock)
            {
                try
                {
                    this.Deserialize(Session.Get(CacheId));
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public void Persist()
        {
            lock (FileLock)
            {
                // reflect changes in the persistent store
                Session.Set(CacheId, this.Serialize());
                // once the write operation took place, restore the HasStateChanged bit to false
                this.HasStateChanged = false;
            }
        }

        // Empties the persistent store.
        public override void Clear()
        {
            base.Clear();
            Session.Remove(CacheId);
        }

        public override void DeleteItem(TokenCacheItem item)
        {
            base.DeleteItem(item);
            Persist();
        }

        // Triggered right before ADAL needs to access the cache.
        // Reload the cache from the persistent store in case it changed since the last access.
        public void BeforeAccessNotification(TokenCacheNotificationArgs args)
        {
            Load();
        }

        // Triggered right after ADAL accessed the cache.
        public void AfterAccessNotification(TokenCacheNotificationArgs args)
        {
            // if the access operation resulted in a cache update
            if (this.HasStateChanged)
            {
                Persist();
            }
        }
    }
}
