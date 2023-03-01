using Project.API.Models;
using Project.WinForms.Models;

namespace Project.WinForms.Mappers
{
    public class StockPositionDiffGridModelUIMapper
    {
        public StockPositionDiffGridUIModel Map(StockPositionDiff data)
        {
            return new StockPositionDiffGridUIModel
            {
                CompanyName = data.CompanyName,
                Shares = data.Shares,
                SharesDiffPercent = data.SharesDiffPercent,
                Ticker = data.Ticker,
                WeightPercent = data.WeightPercent
            };
        }
    }
}
