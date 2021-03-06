﻿using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using AutoMapper.Configuration;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Models;
using IdentityServer4.Test;
using NLog.Fluent;
using ApiResource = IdentityServer4.EntityFramework.Entities.ApiResource;
using Client = IdentityServer4.Models.Client;
using IdentityResource = IdentityServer4.Models.IdentityResource;
using Secret = IdentityServer4.Models.Secret;

namespace Web
{
    public static class IdentityServerConfiguration
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope()
                {
                    ApiResource = new ApiResource(),
                    Name = "qAPI"
                }
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                // machine to machine client
                new Client
                {
                    ClientId = "client",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    // scopes that client has access to
                    AllowedScopes = { "api1" }
                },

                // interactive ASP.NET Core MVC client
                new Client
                {
                    ClientId = "mvc",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                    // where to redirect to after login
                    RedirectUris = { "https://localhost:5002/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                    }
                }
            };

        public static List<TestUser> TestUsers => new List<TestUser>
        {
            new TestUser
            {
                SubjectId = "testId",
                Username = "test@email.com",
                Password = "TestPassword",
                Claims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.Email, "test@email.com"),
                    new Claim(JwtClaimTypes.Role, "TestRole")
                }
            }
        };

        public static X509Certificate2 Certificate(IConfiguration configuration)
        {
            using var certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            certStore.Open(OpenFlags.ReadOnly);
            var certCollection = certStore.Certificates.Find(X509FindType.FindByThumbprint, "CB781679561914B7539BE120EE9C4F6780579A86", false);

            return default;
        }
    }
}