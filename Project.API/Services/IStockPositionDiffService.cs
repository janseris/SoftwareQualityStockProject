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
        /// <item>Equal <see cref="StockPositionRecord.Ticker"/> in <paramref name="positions1"/> and <paramref name="positions2"/></item>
        /// <item>Different dates in <paramref name="positions1"/> and <paramref name="positions2"/></item>
        /// </list>
        /// </summary>
        StockPositionDiff GetDiff(StockPositionRecord positions1, StockPositionRecord positions2);

        /// <summary>
        /// Calculates diff between two lists of stock records.
        /// <br></br>
        /// </summary>
        /// <param name="positions1"></param>
        /// <param name="positions2"></param>
        /// <returns></returns>
        IList<StockPositionDiff> GetDiff(IList<StockPositionRecord> positions1, IList<StockPositionRecord> positions2);
    }
}
