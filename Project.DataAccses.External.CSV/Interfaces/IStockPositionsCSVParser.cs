using Project.API.Models;

namespace Project.DataAccess.External.CSV.Interfaces
{
    public interface IStockPositionsCSVParser
    {
        IList<StockPositionRecord> Parse(byte[] csv);
    }
}
