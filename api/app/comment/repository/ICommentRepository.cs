using api.app.comment.entity;

namespace api.app.comment.repository
{
  public interface ICommentRepository
  {
    Task<List<Comment>> FindAllAsync();
    Task<Comment?> FindByIdAsync(int id);

    Task<Comment> CreateAsync(Comment comment);

    // Task<Comment?> UpdateAsync(int id, UpdateCommentDto dto); 
    Task<Comment?> DeleteByIdAsync(int id);
  }
}