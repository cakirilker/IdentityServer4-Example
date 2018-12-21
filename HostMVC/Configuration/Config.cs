using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace HostMVC
{
    public class Config
    {
        // ApiResources define the apis in your system
        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("employees","Employees Service")
            };
        }

        // Identity resources are data like user ID, name, or email address of a user
        // see: http://docs.identityserver.io/en/release/configuration/resources.html
        public static IEnumerable<IdentityResource> GetResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }

        // Client want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients(Dictionary<string, string> clientUrl)
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId= "mvc",
                    ClientName="MVC Client",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },
                    ClientUri = $"{clientUrl["Mvc"]}",
                    AllowedGrantTypes=GrantTypes.Hybrid,
                    AllowAccessTokensViaBrowser=false,
                    RequireConsent=false,
                    AllowOfflineAccess=true,
                    AlwaysIncludeUserClaimsInIdToken=true,
                    RedirectUris=new List<string>
                    {
                        $"{clientUrl["Mvc"]}/signin-oidc"
                    },
                    FrontChannelLogoutUri = $"{clientUrl["Mvc"]}/signout-oidc",
                    PostLogoutRedirectUris=new List<string>
                    {
                        $"{clientUrl["Mvc"]}/signout-callback-oidc"
                    },
                    AllowedScopes=new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "employees"
                    },
                    AccessTokenLifetime=60*60*2,
                    IdentityTokenLifetime=60*60*2
                },
                 new Client
                {
                    ClientId= "second_mvc_client",
                    ClientName="Second MVC Client",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },
                    ClientUri = $"{clientUrl["SecondMvc"]}",
                    AllowedGrantTypes=GrantTypes.Hybrid,
                    AllowAccessTokensViaBrowser=false,
                    RequireConsent=false,
                    AllowOfflineAccess=true,
                    AlwaysIncludeUserClaimsInIdToken=true,
                    RedirectUris=new List<string>
                    {
                        $"{clientUrl["SecondMvc"]}/signin-oidc"
                    },
                    FrontChannelLogoutUri = $"{clientUrl["SecondMvc"]}/signout-oidc",
                    PostLogoutRedirectUris=new List<string>
                    {
                        $"{clientUrl["SecondMvc"]}/signout-callback-oidc"
                    },
                    AllowedScopes=new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OfflineAccess
                    },
                    AccessTokenLifetime=60*60*2,
                    IdentityTokenLifetime=60*60*2
                }
            };
        }
    }
}
