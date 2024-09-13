using System.ComponentModel.DataAnnotations;

namespace api.app.comment.dto {
  public class UpdateCommentDto {
    [Required]
    public string Title { get; set; } = string.Empty;
    [Required]
    public string Content { get; set; } = string.Empty;
  }
}