using Project.API.Models;
using Project.API.Services;
using Project.DataAccess.External.CSV.Interfaces;

namespace Project.Logic.Services
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

        /// <summary>
        /// For testing purposes
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<IList<StockPositionRecord>> GetTodaysRecordsFromCSVFile(byte[] file)
        {
            return csvParser.Parse(file);
        }
    }
}
