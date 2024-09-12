using api.app.comment.dto;
using api.app.comment.entity;

namespace api.app.comment.mapper {
  public static class CommentMapper {
    public static CommentDto toCommentDto(this Comment comment) {
      return new CommentDto {
        Id = comment.Id,
        Content = comment.Content,
        CreatedOn = comment.CreatedOn,
        Title = comment.Title,
        StockId = comment.StockId
      };
    }

    public static Comment toComment(this CreateCommentDto dto, int stockId) {
      return new Comment {
        Content = dto.Content,
        Title = dto.Title,
        StockId = stockId
      };
    }
  }
}