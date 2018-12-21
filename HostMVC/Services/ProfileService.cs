using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HostMVC.Infrastructure;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;

namespace HostMVC.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));
            var subjectId = subject.Claims.Where(x => x.Type == "sub").FirstOrDefault().Value;

            var user = await _userManager.FindByIdAsync(subjectId);
            if (user == null)
            {
                throw new ArgumentException("Invalid Sid");
            }
            context.AddRequestedClaims(context.Subject.Claims);
            context.IssuedClaims = null;
            IEnumerable<Claim> claims = GetClaimsFromUser(user);
            context.IssuedClaims = claims.ToList();
        }

        private IEnumerable<Claim> GetClaimsFromUser(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject,user.Id),
                new Claim(JwtClaimTypes.PreferredUserName,user.UserName),
                new Claim(JwtRegisteredClaimNames.UniqueName,user.UserName)
            };

            if (!string.IsNullOrEmpty(user.Firstname))
                claims.Add(new Claim("firstname", user.Firstname));
            if (!string.IsNullOrEmpty(user.Lastname))
                claims.Add(new Claim("lastname", user.Lastname));
            if (!string.IsNullOrEmpty(user.Country))
                claims.Add(new Claim("country", user.Country));

            if (_userManager.SupportsUserEmail)
            {
                claims.AddRange(new[] {
                    new Claim(JwtClaimTypes.Email,user.Email),
                    new Claim(JwtClaimTypes.EmailVerified,user.EmailConfirmed.ToString().ToLower(),ClaimValueTypes.Boolean)
                });
            }

            return claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));
            var subjectId = subject.Claims.Where(x => x.Type == "sub").FirstOrDefault().Value;
            var user = await _userManager.FindByIdAsync(subjectId);

            context.IsActive = false;
            if (user != null)
            {
                if (_userManager.SupportsUserSecurityStamp)
                {
                    var securityStamp = subject.Claims.Where(c => c.Type == "security_stamp").Select(c => c.Value).SingleOrDefault();
                    if (securityStamp != null)
                    {
                        var storeSecurityStamp = await _userManager.GetSecurityStampAsync(user);
                        if (storeSecurityStamp != securityStamp)
                        {
                            return;
                        }
                    }
                }
                context.IsActive = !user.LockoutEnabled || !user.LockoutEnd.HasValue || user.LockoutEnd <= DateTime.Now;
            }
        }
    }
}
