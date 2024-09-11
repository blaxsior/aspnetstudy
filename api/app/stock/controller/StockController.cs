using api.app.db;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using api.app.stock.dto;
using api.app.stock.mappers;

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
    public IActionResult GetAll()
    {
      var stocks = context.Stocks.Select(s => s.ToStockDto()).ToList();

      return Ok(stocks);
    }

    [HttpGet("{id}")]
    public IActionResult Get([FromRoute(Name = "id")] int id)
    {
      var stock = context.Stocks.FirstOrDefault(x => x.Id == id);
      if (stock is null) return NotFound();
      return Ok(stock.ToStockDto());
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateStockRequest request) {
      var stock = request.ToStock();
      context.Stocks.Add(stock);
      context.SaveChanges();
      // 생성 표현 목적의 Action
      // return CreatedAtAction(nameof(Get), new {id = stock.Id}, stock);
      return Created();
    }

    [HttpPut("{id}")]
    public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequest request) {
      var stock = context.Stocks.FirstOrDefault(x => x.Id == id);
      if (stock is null) return NotFound();

      stock.Symbol = request.Symbol;
      stock.CompanyName = request.CompanyName;
      stock.Purchase = request.Purchase;
      stock.LastDiv = request.LastDiv;
      stock.Industry = request.Industry;
      stock.MarketCap = request.MarketCap;

      // 변경사항 반영
      context.SaveChanges();

      return Ok();
    }
  }
}