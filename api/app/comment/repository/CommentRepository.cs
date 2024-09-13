using api.app.comment.dto;
using api.app.comment.entity;
using api.app.db;
using Microsoft.EntityFrameworkCore;

namespace api.app.comment.repository
{
  public class CommentRepository : ICommentRepository
  {
    private readonly ApplicationDbContext context;
    public CommentRepository(ApplicationDbContext context)
    {
      this.context = context;
    }

    public Task<List<Comment>> FindAllAsync()
    {
      return context.Comments.ToListAsync();
    }
    public async Task<Comment?> FindByIdAsync(int id)
    {
      var comment = await context.Comments.FindAsync(id);
      return comment;
    }

    public async Task<Comment> CreateAsync(Comment comment)
    {
      await context.Comments.AddAsync(comment);
      await context.SaveChangesAsync();

      return comment;
    }
    public async Task<Comment?> UpdateAsync(int id, UpdateCommentDto dto)
    {
      var comment = await context.Comments.FindAsync(id);
      if(comment is null) return null;

      comment.Title = dto.Title;
      comment.Content = dto.Content;

      await context.SaveChangesAsync();

      return comment;
    }

    public async Task<Comment?> DeleteByIdAsync(int id)
    {
      var comment = await context.Comments.FindAsync(id);
      if (comment is null) return null;

      context.Comments.Remove(comment);
      context.SaveChanges();

      return comment;
    }
  }
}