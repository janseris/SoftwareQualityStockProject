using System;
using System.Collections.Generic;

using Project.API.Models;

namespace Project.API.DataAccess
{
    /// <summary>
    /// Manages local persistent data source where records for various dates are stored to be able to calculate diffs from them
    /// </summary>
    public interface IStockPositionsLocalDAO
    {
        IList<StockPositionRecord> GetAll();

        IList<StockPositionRecord> GetAll(List<DateTime> dates);

        IList<StockPositionRecord> GetAll(DateTime date);

        bool AnyRecordsExist(DateTime date);

        /// <summary>
        /// Adds <paramref name="records"/> to persistent storage.
        /// <br>Preconditions:</br>
        /// <list type="bullet">
        /// <item>All <paramref name="records"/> are of the same date</item>
        /// <item>All records <paramref name="records"/> are unique (by <see cref="StockPositionRecord.CompanyName"/> and <see cref="StockPositionRecord.Ticker"/>) (no duplicate records for the same copmany)</item>
        /// </list>
        /// </summary>
        /// <param name="records"></param>
        void InsertAll(IList<StockPositionRecord> records);
    }
}
