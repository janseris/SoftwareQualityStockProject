using Project.API.DataAccess;
using Project.API.Facades;
using Project.API.Models;
using Project.API.Services;

namespace Project.Logic.Facades
{
    public class StockPositionsAdministrationFacade : IStockPositionsAdministrationFacade
    {
        private readonly IStockPositionsLocalDAO localDAO;
        private readonly ITodaysStockPositionsLoadingService sourceDAO;

        public StockPositionsAdministrationFacade(IStockPositionsLocalDAO localDAO, ITodaysStockPositionsLoadingService sourceDAO)
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

            if(existsForToday && overwrite)
            {
                //TODO: transaction
                await localDAO.DeleteForDate(DateTime.Now);
                await localDAO.InsertAll(data);
                return true;
            }
            if (existsForToday == false)
            {
                await localDAO.InsertAll(data);
                return true;
            }
            return false;
        }
    }
}
