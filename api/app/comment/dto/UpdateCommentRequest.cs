using System.ComponentModel.DataAnnotations;

namespace api.app.comment.dto {
  public class UpdateCommentDto {
    [Required]
    [MinLength(5, ErrorMessage = "Title must be at least 5 char")]
    [MaxLength(280, ErrorMessage = "Title cannot be over 280 char")]
    public string Title { get; set; } = string.Empty;
    [Required]
    [MinLength(5, ErrorMessage = "Content must be at least 5 char")]
    [MaxLength(280, ErrorMessage = "Content cannot be over 280 char")]
    public string Content { get; set; } = string.Empty;
  }
}