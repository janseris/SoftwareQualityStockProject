using Microsoft.EntityFrameworkCore;

using Project.API;
using Project.API.DataAccess;
using Project.API.Models;
using Project.DataAccess.Local.SQLServer.Models;

namespace Project.DataAccess.Local.SQLServer
{
    public class StockPositionsLocalDAO : IStockPositionsLocalDAO
    {
        private readonly IDbContextFactory<SWQualityProjectContext> dbContextFactory;
        private readonly StockPositionInsertDBMapper insertMapper;
        private SWQualityProjectContext DB => dbContextFactory.CreateDbContext();

        public StockPositionsLocalDAO(IDbContextFactory<SWQualityProjectContext> dbContextFactory, StockPositionInsertDBMapper insertMapper)
        {
            this.dbContextFactory = dbContextFactory;
            this.insertMapper = insertMapper;
        }

        public async Task<bool> AnyRecordsExist(DateTime date)
        {
            var dateOnly = DateTimeHelper.KeepOnlyYearMonthDay(date);

            using var db = DB;
            var query =
                from item in db.STOCK_POSITION
                where item.Date == date
                select 1;
            return await query.AnyAsync();
        }


        public async Task<IList<StockPositionRecord>> GetAll()
        {
            using var db = DB;
            var query = from item in db.STOCK_POSITION
                        select new StockPositionRecord(item.Date, item.CompanyName, item.Ticker, item.Shares, item.WeightPercent);
            var items = await query.AsNoTracking().ToListAsync();
            return items;
        }

        public async Task<IList<StockPositionRecord>> GetAll(IList<DateTime> dates)
        {
            var datesOnly = DateTimeHelper.KeepOnlyYearMonthDay(dates);

            using var db = DB;
            var query = from item in db.STOCK_POSITION
                        where datesOnly.Contains(item.Date)
                        select new StockPositionRecord(item.Date, item.CompanyName, item.Ticker, item.Shares, item.WeightPercent);
            var items = await query.AsNoTracking().ToListAsync();
            return items;
        }

        public async Task<IList<StockPositionRecord>> GetAll(DateTime date)
        {
            var dateOnly = DateTimeHelper.KeepOnlyYearMonthDay(date);

            using var db = DB;
            var query = from item in db.STOCK_POSITION
                        where item.Date == dateOnly
                        select new StockPositionRecord(item.Date, item.CompanyName, item.Ticker, item.Shares, item.WeightPercent);
            var items = await query.AsNoTracking().ToListAsync();
            return items;
        }

        public async Task<IList<DateTime>> GetAvailableDates()
        {
            using var db = DB;
            var query = from item in db.STOCK_POSITION
                        select item.Date;
            var items = await query.Distinct().ToListAsync();
            return items;
        }

        /// <summary>
        /// Does not check if records for current day already exist
        /// </summary>
        public async Task InsertAll(IList<StockPositionRecord> records)
        {
            var items = (
                from record in records
                select insertMapper.Map(record)
                ).ToList();
            using var db = DB;
            db.AddRange(items);
            await db.SaveChangesAsync();
        }

        public async Task DeleteForDate(DateTime date)
        {
            var dateOnly = DateTimeHelper.KeepOnlyYearMonthDay(date);
            using var db = DB;

            //EF Core 7 bulk delete could be used without roundtrip maybe

            var query = from item in db.STOCK_POSITION
                        where item.Date == dateOnly
                        select item;
            var items = await query.ToListAsync();
            db.RemoveRange(items);
            await db.SaveChangesAsync();
        }
    }
}
