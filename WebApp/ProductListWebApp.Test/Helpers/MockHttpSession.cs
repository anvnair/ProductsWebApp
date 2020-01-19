using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;
using ProductListWebApp.Models;
using ProductListWebApp.Services;
using ProductListWebApp.Utils;
using ProductWebApp.Controllers;
using ProductWebApp.Models;

namespace ProductControllerWebApp.Test
{
    public class MockHttpSession : ISession
    {
        Dictionary<string, object> sessionStorage = new Dictionary<string, object>();

        public object this[string name]
        {
            get { return sessionStorage[name]; }
            set { sessionStorage[name] = value; }
        }

        string ISession.Id
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        bool ISession.IsAvailable
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        IEnumerable<string> ISession.Keys
        {
            get { return sessionStorage.Keys; }
        }

        public Task CommitAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task LoadAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        void ISession.Clear()
        {
            sessionStorage.Clear();
        }

        //Task ISession.CommitAsync()
        //{
        //    throw new NotImplementedException();
        //}

        //Task ISession.LoadAsync()
        //{
        //    throw new NotImplementedException();
        //}

        void ISession.Remove(string key)
        {
            sessionStorage.Remove(key);
        }

        void ISession.Set(string key, byte[] value)
        {
            sessionStorage[key] = value;
        }

        bool ISession.TryGetValue(string key, out byte[] value)
        {
            if (sessionStorage[key] != null)
            {
                value = Encoding.ASCII.GetBytes(sessionStorage[key].ToString());
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }
    }
}