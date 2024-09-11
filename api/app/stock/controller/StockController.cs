using api.app.db;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using api.app.stock.dto;
using api.app.stock.mappers;
using Microsoft.EntityFrameworkCore;

namespace api.app.stock.controller
{
  [Route("api/stock")]
  public class StockController : ControllerBase
  {
    private readonly ApplicationDbContext context;

    public StockController(ApplicationDbContext context)
    {
      this.context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
      var stocks = await context.Stocks.Select(s => s.ToStockDto()).ToListAsync();

      return Ok(stocks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute(Name = "id")] int id)
    {
      var stock = await context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
      if (stock is null) return NotFound();
      return Ok(stock.ToStockDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockRequest request) {
      var stock = request.ToStock();
      await context.Stocks.AddAsync(stock);
      await context.SaveChangesAsync();
      // 생성 표현 목적의 Action
      // return CreatedAtAction(nameof(Get), new {id = stock.Id}, stock);
      return Created();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequest request) {
      var stock = await context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
      if (stock is null) return NotFound();

      stock.Symbol = request.Symbol;
      stock.CompanyName = request.CompanyName;
      stock.Purchase = request.Purchase;
      stock.LastDiv = request.LastDiv;
      stock.Industry = request.Industry;
      stock.MarketCap = request.MarketCap;

      // 변경사항 반영
      await context.SaveChangesAsync();

      return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id) {
      var stock = await context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
      if(stock is null) return NotFound();

      context.Stocks.Remove(stock);
      await context.SaveChangesAsync();
      return NoContent();
    }
  }

  
}