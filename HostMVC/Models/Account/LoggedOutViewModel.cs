using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HostMVC.Models.Account
{
    public class LoggedOutViewModel
    {
        public string PostLogoutRedirectUri { get; set; }
        public string ClientName { get; set; }
        public string SignOutIframeUrl { get; set; }
        public bool AutomaticRedirectAfterSignOut { get; set; } = false;
        public string LogoutId { get; set; }
        public bool TriggerExternalSignout => ExternalAuthenticationSchema != null;
        public string ExternalAuthenticationSchema { get; set; }
    }
}
