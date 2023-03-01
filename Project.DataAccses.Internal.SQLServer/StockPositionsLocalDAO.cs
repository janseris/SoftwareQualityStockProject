﻿using Microsoft.EntityFrameworkCore;

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
            using var db = DB;
            return await db.STOCK_POSITION.AnyAsync(); //SELECT
        }

        private IQueryable<StockPositionRecord> CreateGetQuery(SWQualityProjectContext db)
        {
            return from item in db.STOCK_POSITION
                   select new StockPositionRecord(item.Date, item.CompanyName, item.Ticker, item.Shares, item.WeightPercent);
        }

        public async Task<IList<StockPositionRecord>> GetAll()
        {
            using var db = DB;
            var query = CreateGetQuery(db);
            var items = await query.AsNoTracking().ToListAsync(); //SELECT
            return items;
        }

        public async Task<IList<StockPositionRecord>> GetAll(List<DateTime> dates)
        {
            var dateonlys = (from date in dates select date.Date).ToList(); //TODO: check if needed to function properly

            using var db = DB;
            var query = CreateGetQuery(db);
            query = from item in query where dates.Contains(item.Date) select item; //WHERE
            var items = await query.AsNoTracking().ToListAsync(); //SELECT
            return items;
        }

        public async Task<IList<StockPositionRecord>> GetAll(DateTime date)
        {
            using var db = DB;
            var query = CreateGetQuery(db);
            query = from item in query where item.Date == date select item; //WHERE
            var items = await query.AsNoTracking().ToListAsync(); //SELECT
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

        public async Task InsertAll(IList<StockPositionRecord> records)
        {
            var items = (from record in records select insertMapper.Map(record)).ToList();
            using var db = DB;
            db.AddRange(items);
            await db.SaveChangesAsync(); //INSERT
        }

        public async Task DeleteForDate(DateTime date)
        {
            using var db = DB;

            //EF Core 7 bulk delete could be used without roundtrip maybe

            var query = from item in db.STOCK_POSITION where item.Date == date select item;
            var items = await query.ToListAsync(); //SELECT
            db.RemoveRange(items);
            await db.SaveChangesAsync(); //DELETE
        }
    }
}
