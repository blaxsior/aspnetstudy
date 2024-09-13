using api.app.stock.dto;
using api.app.stock.entity;

namespace api.app.stock.repository {
  public interface IStockRepository {
    Task<List<Stock>> FindAllAsync(ListStockQuery queries);
    Task<Stock?> FindByIdAsync(int id);

    Task<Stock> CreateAsync(Stock stock);

    Task<Stock?> UpdateAsync(int id, UpdateStockDto dto); 
    Task<Stock?> DeleteByIdAsync(int id);

    Task<bool> ExistsAsync(int id);
  }
}