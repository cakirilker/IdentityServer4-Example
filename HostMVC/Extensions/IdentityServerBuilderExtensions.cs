using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace HostMVC.Certificate
{
    static class IdentityServerBuilderExtensions
    {
        public static IIdentityServerBuilder AddSigningCredential(this IIdentityServerBuilder builder, IConfigurationSection options)
        {
            var keyFilePath = options.GetValue<string>("KeyFilePath");
            var keyFilePassword = options.GetValue<string>("KeyFilePassword");
            builder.AddSigningCredential(new X509Certificate2(keyFilePath, keyFilePassword));
            return builder;
        }
    }
}