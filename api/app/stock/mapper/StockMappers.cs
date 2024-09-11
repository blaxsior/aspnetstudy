using api.app.stock.dto;
using api.app.stock.entity;

namespace api.app.stock.mappers {
  public static class StockMappers {
    public static StockDto ToStockDto(this Stock stock) {
      return new StockDto {
        Id = stock.Id,
        Symbol = stock.Symbol,
        CompanyName = stock.CompanyName,
        Purchase = stock.Purchase,
        LastDiv = stock.LastDiv,
        Industry = stock.Industry,
        MarketCap = stock.MarketCap
      };
    }

    public static Stock ToStock(this CreateStockRequest request) {
      return new Stock {
        Symbol = request.Symbol,
        CompanyName = request.CompanyName,
        Purchase = request.Purchase,
        LastDiv = request.LastDiv,
        Industry = request.Industry,
        MarketCap = request.MarketCap
      };
    }
  }
}