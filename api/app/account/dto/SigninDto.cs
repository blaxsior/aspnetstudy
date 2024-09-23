using System.ComponentModel.DataAnnotations;

namespace api.app.account.dto {
  public class SigninDto {
    [Required]
    public required String Username { get; set; }
    [Required]
    public required String Password { get; set; }
  }
}