using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace BtcKpi.Web.Extensions
{
    public static class ClaimsExtensions
    {
        static string GetUserEmail(this ClaimsIdentity identity)
        {
            return identity.Claims?.FirstOrDefault(c => c.Type == "BtcKpi.Web.Models.RegisterViewModel.Email")?.Value;
        }

        public static string GetUserEmail(this IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            return claimsIdentity != null ? GetUserEmail(claimsIdentity) : "";
        }

        static string GetUserNameIdentifier(this ClaimsIdentity identity)
        {
            return identity.Claims?.FirstOrDefault(c => c.Type == "BtcKpi.Web.Models.RegisterViewModel.NameIdentifier")?.Value;
        }

        public static string GetUserNameIdentifier(this IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            return claimsIdentity != null ? GetUserNameIdentifier(claimsIdentity) : "";
        }
    }
}