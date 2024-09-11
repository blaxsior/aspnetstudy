using api.app.stock.dto;
using api.app.stock.entity;

namespace api.app.stock.repository {
  public interface IStockRepository {
    Task<List<Stock>> FindAllAsync();
    Task<Stock?> FindByIdAsync(int id);

    Task<Stock> CreateAsync(Stock stock);

    Task<Stock?> UpdateAsync(int id, UpdateStockDto dto); 
    Task<Stock?> DeleteByIdAsync(int id);
  }
}