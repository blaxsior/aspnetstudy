using api.app.stock.dto;
using api.app.stock.entity;

namespace api.app.stock.mappers {
  // java와 달리 확장 메서드가 존재하므로 dto나 엔티티 외부에 mapper 로직 작성 가능
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