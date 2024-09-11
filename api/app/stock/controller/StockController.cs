using api.app.db;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using api.app.stock.dto;
using api.app.stock.mappers;
using Microsoft.EntityFrameworkCore;
using api.app.stock.repository;

namespace api.app.stock.controller
{
  [Route("api/stock")]
  public class StockController : ControllerBase
  {
    private readonly ApplicationDbContext context;
    private readonly IStockRepository stockRepo;

    public StockController(ApplicationDbContext context, IStockRepository stockRepo)
    {
      this.context = context;
      this.stockRepo = stockRepo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
      var stocks = await stockRepo.FindAllAsync();

      return Ok(stocks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute(Name = "id")] int id)
    {
      var stock = await stockRepo.FindByIdAsync(id);
      if (stock is null) return NotFound();
      return Ok(stock.ToStockDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockRequest request) {
      var stock = await stockRepo.CreateAsync(request.ToStock());
      // 생성 표현 목적의 Action
      return CreatedAtAction(nameof(Get), new {id = stock.Id}, stock);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockDto request) {
      var stock = await stockRepo.UpdateAsync(id, request);

      if(stock is null) return NotFound();    
      return Ok(stock.ToStockDto());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id) {
      var stock = await stockRepo.DeleteByIdAsync(id);
      if(stock is null) return NotFound();
      
      return NoContent();
    }
  }

  
}