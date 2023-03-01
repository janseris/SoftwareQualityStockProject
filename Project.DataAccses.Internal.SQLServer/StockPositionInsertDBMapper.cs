using Project.API.Models;
using Project.DataAccess.Local.SQLServer.Models;

namespace Project.DataAccess.Local.SQLServer
{
    public class StockPositionInsertDBMapper
    {
        public STOCK_POSITION Map(StockPositionRecord data)
        {
            return new STOCK_POSITION
            {
                Date = data.Date,
                CompanyName = data.CompanyName,
                Ticker = data.Ticker,
                Shares = data.Shares,
                WeightPercent = data.WeightPercent,
            };
        }
    }
}
