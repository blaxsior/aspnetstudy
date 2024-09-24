using api.app.comment.dto;
using api.app.comment.mapper;
using api.app.comment.repository;
using api.app.stock.repository;
using api.app.user.entity;
using api.extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.app.comment.controller
{
  [Route("api/comment")]
  [ApiController]
  public class CommentController : ControllerBase
  {
    private readonly ICommentRepository commentRepo;
    private readonly IStockRepository stockRepo;
    private readonly UserManager<AppUser> userManager;


    public CommentController(
      ICommentRepository commentRepo, 
    IStockRepository stockRepo,
    UserManager<AppUser> userManager
    )
    {
      this.commentRepo = commentRepo;
      this.stockRepo = stockRepo;
      this.userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> FindAll()
    {
      // API 모드에서는 사용 안해도 됨
      // 각 요청마다 새로운 ModelState 만들어 사용
      // if(!ModelState.IsValid) {
      //   return BadRequest(ModelState);
      // }

      var comments = await commentRepo.FindAllAsync();
      var dtos = comments.Select(it => it.toCommentDto());
      return Ok(dtos);
    }

    // type checking 실패하면 404 예외 반환
    [HttpGet("{id:int}")]
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

      var username = User.GetUsername();
      var appUser = await userManager.FindByNameAsync(username);

      var commentModel = dto.toComment(stockId);
      commentModel.AppUserId = appUser?.Id!;

      var comment = await commentRepo.CreateAsync(commentModel);
      if (comment is null) return NotFound();
      return CreatedAtAction(nameof(FindById), new { id = comment.Id }, comment.toCommentDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentDto dto)
    {
      var comment = await commentRepo.UpdateAsync(id, dto);

      if (comment is null) return BadRequest("Comment Not Exists");
      return Ok(comment);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
      var comment = await commentRepo.DeleteByIdAsync(id);

      if (comment is null) return BadRequest("Comment Not Exists");
      return Ok(comment);
    }
  }
}