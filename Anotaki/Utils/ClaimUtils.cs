using System.Security.Claims;

namespace anotaki_api.Utils
{
    public static class ClaimUtils
    {
        public static int? GetUserId(ClaimsPrincipal user)
        {
            var idClaim = user.FindFirst(ClaimTypes.NameIdentifier) ?? user.FindFirst("sub");
            if (idClaim == null || !int.TryParse(idClaim.Value, out var userId))
            {
                return null;
            }

            return userId;
        }
    }
}
