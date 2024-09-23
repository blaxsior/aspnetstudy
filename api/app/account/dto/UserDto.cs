using System.ComponentModel.DataAnnotations;

namespace api.app.account.dto {
  public class UserDto {
    public required string Username {get; init;}

    public required string Email {get; init;}

    public required string Token {get; init;}
  }
}