using Project.API.DataAccess;
using Project.API.Models;
using Project.API.Services;

namespace Project.Services
{
    public class StockPositionsAdministrationService : IStockPositionsAdministrationFacade
    {
        private readonly IStockPositionsLocalDAO localDAO;
        private readonly ITodaysStockPositionsLoadingService sourceDAO;

        public StockPositionsAdministrationService(IStockPositionsLocalDAO localDAO, ITodaysStockPositionsLoadingService sourceDAO)
        {
            this.localDAO = localDAO;
            this.sourceDAO = sourceDAO;
        }

        public async Task<bool> AreTodaysStockPositionsAlreadySaved()
        {
            return await localDAO.AnyRecordsExist(DateTime.Now);
        }

        public async Task<IList<StockPositionRecord>> GetAll(DateTime date)
        {
            return await localDAO.GetAll(date);
        }

        public async Task<IList<DateTime>> GetAvailableDates()
        {
            return await localDAO.GetAvailableDates();
        }

        public async Task<bool> SaveTodaysStockPositions(bool overwrite)
        {
            var data = await sourceDAO.GetTodaysRecords();
            var existsForToday = await localDAO.AnyRecordsExist(DateTime.Now);

            bool insertNotNeeded = existsForToday && !overwrite;
            if (insertNotNeeded)
            {
                return false;
            }
            await localDAO.InsertAll(data);
            return true;
        }
    }
}
