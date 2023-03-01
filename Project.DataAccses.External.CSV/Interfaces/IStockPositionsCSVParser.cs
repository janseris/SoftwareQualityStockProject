using Project.API.Models;

namespace Project.DataAccses.External.CSV.Interfaces
{
    public interface IStockPositionsCSVParser
    {
        IList<StockPositionRecord> Parse(byte[] csv);
    }
}
