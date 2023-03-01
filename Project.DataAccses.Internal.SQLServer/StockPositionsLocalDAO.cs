using Project.API.DataAccess;
using Project.API.Models;

namespace Project.DataAccess.Local.SQLServer
{
    public class StockPositionsLocalDAO : IStockPositionsLocalService
    {
        public bool AnyRecordsExist(DateTime date)
        {
            throw new NotImplementedException();
        }

        public IList<StockPositionRecord> GetAll()
        {
            throw new NotImplementedException();
        }

        public IList<StockPositionRecord> GetAll(List<DateTime> dates)
        {
            throw new NotImplementedException();
        }

        public IList<StockPositionRecord> GetAll(DateTime date)
        {
            throw new NotImplementedException();
        }

        public void InsertAll(IList<StockPositionRecord> records)
        {
            throw new NotImplementedException();
        }
    }
}
