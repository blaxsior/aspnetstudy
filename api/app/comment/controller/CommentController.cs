using api.app.comment.dto;
using api.app.comment.mapper;
using api.app.comment.repository;
using api.app.stock.repository;
using Microsoft.AspNetCore.Mvc;

namespace api.app.comment.controller
{
  [Route("api/comment")]
  [ApiController]
  public class CommentController : ControllerBase
  {
    private readonly ICommentRepository commentRepo;
    private readonly IStockRepository stockRepo;

    public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo)
    {
      this.commentRepo = commentRepo;
      this.stockRepo = stockRepo;
    }

    [HttpGet]
    public async Task<IActionResult> FindAll()
    {
      var comments = await commentRepo.FindAllAsync();
      var dtos = comments.Select(it => it.toCommentDto());
      return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> FindById([FromRoute] int id)
    {
      var comment = await commentRepo.FindByIdAsync(id);
      if (comment is null) return NotFound();
      return Ok(comment.toCommentDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromQuery] int stockId, [FromBody] CreateCommentDto dto)
    {
      if (!await stockRepo.ExistsAsync(stockId)) return BadRequest("Stock Not Exists");

      var comment = await commentRepo.CreateAsync(dto.toComment(stockId));
      if (comment is null) return NotFound();
      return CreatedAtAction(nameof(FindById), new { id = comment.Id }, comment.toCommentDto());
    }
  }
}