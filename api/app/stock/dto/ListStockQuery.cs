namespace api.app.stock.dto
{
  public class ListStockQuery
  {
    public string? Symbol { get; set; } = null;
    public string? CompanyName { get; set; } = null;
    public string? SortBy {get; set;} = null;
    public bool IsDesc { get; set;} = false;
  }
}