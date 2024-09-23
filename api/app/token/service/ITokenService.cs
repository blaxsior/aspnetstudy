using api.app.user.entity;

namespace api.app.token.service {
  public interface ITokenService {
    string CreateToken(AppUser user); 
  }
}