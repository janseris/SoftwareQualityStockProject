using Project.API.DataAccess;
using Project.API.Models;
using Project.DataAccses.External.CSV.Interfaces;

namespace Project.Services
{
    public class TodaysStockPositionsLoadingService : ITodaysStockPositionsLoadingService
    {
        private readonly IStockPositionsCSVDAO csvDAO;
        private readonly IStockPositionsCSVParser csvParser;

        public TodaysStockPositionsLoadingService(IStockPositionsCSVDAO csvDAO, IStockPositionsCSVParser csvParser)
        {
            this.csvDAO = csvDAO;
            this.csvParser = csvParser;
        }

        public async Task<IList<StockPositionRecord>> GetTodaysRecords()
        {
            var csv = await csvDAO.GetTodayRecordsCSV();
            var items = csvParser.Parse(csv);
            return items;
        }
    }
}
