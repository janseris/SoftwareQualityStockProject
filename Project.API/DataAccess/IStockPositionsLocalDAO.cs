using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Project.API.Models;

namespace Project.API.DataAccess
{
    /// <summary>
    /// Manages local persistent data source where records for various dates are stored to be able to calculate diffs from them
    /// </summary>
    public interface IStockPositionsLocalDAO
    {
        Task<IList<StockPositionRecord>> GetAll();

        Task<IList<StockPositionRecord>> GetAll(List<DateTime> dates);

        Task<IList<StockPositionRecord>> GetAll(DateTime date);

        Task<bool> AnyRecordsExist(DateTime date);

        /// <summary>
        /// Returns distinct date values which are present in the data source -> a list of dates which we can use for diffs.
        /// </summary>
        Task<IList<DateTime>> GetAvailableDates();

        /// <summary>
        /// Adds <paramref name="records"/> to persistent storage.
        /// <br>Preconditions:</br>
        /// <list type="bullet">
        /// <item>All <paramref name="records"/> are of the same date</item>
        /// <item>All records <paramref name="records"/> are unique (by <see cref="StockPositionRecord.CompanyName"/> and <see cref="StockPositionRecord.Ticker"/>) (no duplicate records for the same copmany)</item>
        /// </list>
        /// </summary>
        /// <param name="records"></param>
        Task InsertAll(IList<StockPositionRecord> records);
    }
}
