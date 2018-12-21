using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HostMVC.Infrastructure
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Country { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
    }
}
