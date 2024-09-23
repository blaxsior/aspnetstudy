using System.Security.Claims;

namespace api.extensions {
  public static class ClaimsExtensions {
    private static string claim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname";
    public static string GetUsername(this ClaimsPrincipal user) {
      return user.Claims.SingleOrDefault(it => it.Type.Equals(claim)).Value;
    }
  }
}