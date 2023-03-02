using Project.API.DataAccess;
using Project.API.Facades;
using Project.API.Models;
using Project.API.Services;

namespace Project.Logic.Facades
{
    public class StockPositionsDiffFacade : IStockPositionsDiffFacade
    {
        private readonly IStockPositionsLocalDAO dao;
        private readonly IStockPositionDiffService service;

        public StockPositionsDiffFacade(IStockPositionsLocalDAO dao, IStockPositionDiffService service)
        {
            this.dao = dao;
            this.service = service;
        }

        public async Task<bool> DataExists(DateTime date)
        {
            return await dao.AnyRecordsExist(date);
        }

        public async Task<IList<StockPositionDiff>> GetDiff(DateTime date1, DateTime date2)
        {
            await ThrowIfNoRecords(date1);
            await ThrowIfNoRecords(date2);

            var date1Records = await dao.GetAll(date1);
            var date2Records = await dao.GetAll(date2);

            var diff = service.GetDiff(date1Records, date2Records);
            return diff;
        }

        private async Task ThrowIfNoRecords(DateTime date)
        {
            if (await dao.AnyRecordsExist(date) == false)
            {
                throw new ArgumentException($"No data for date {date}");
            }
        }
    }
}
