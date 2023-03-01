using System.Collections.Generic;

using Project.API.Models;

namespace Project.API.Services
{
    /// <summary>
    /// Does not use any data source
    /// </summary>
    public interface IStockPositionDiffService
    {
        /// <summary>
        /// Creates a diff off of two records of the same company for two different dates.
        /// <br>Preconditions:</br>
        /// <list type="bullet">
        /// <item>Equal <see cref="StockPositionRecord.Ticker"/> in <paramref name="info1"/> and <paramref name="info2"/></item>
        /// <item>Different dates in <paramref name="info1"/> and <paramref name="info2"/></item>
        /// </list>
        /// </summary>
        StockPositionDiff GetDiff(StockPositionRecord info1, StockPositionRecord info2);

        /// <summary>
        /// Calculates diff between two lists of stock records.
        /// <br></br>
        /// </summary>
        /// <param name="info1"></param>
        /// <param name="info2"></param>
        /// <returns></returns>
        IList<StockPositionDiff> GetDiff(IList<StockPositionRecord> info1, IList<StockPositionRecord> info2);
    }
}
