using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.app.user.entity;
using Microsoft.IdentityModel.Tokens;

namespace api.app.token.service {
  public class TokenService: ITokenService {
    private readonly IConfiguration config;
    private readonly SymmetricSecurityKey key;
    public TokenService(IConfiguration config) {
      this.config = config;
      this.key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:SigningKey"]));
    }

    public string CreateToken(AppUser user) {
      var claims = new List<Claim> {
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim(JwtRegisteredClaimNames.GivenName, user.UserName)
      };

      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
      var tokenDescriptor = new SecurityTokenDescriptor {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = creds,
        Issuer = config["JWT:Issuer"],
        Audience = config["JWT:Audience"]
      };

      var tokenHandler = new JwtSecurityTokenHandler();
      var token = tokenHandler.CreateToken(tokenDescriptor);

      return tokenHandler.WriteToken(token);
    }
  }
}