using System;
using System.Collections.Generic;

using Project.API.Models;

namespace Project.API.Facades
{
    /// <summary>
    /// What end-user is using
    /// </summary>
    public interface IStockPositionsDiffFacade
    {
        /// <summary>
        /// <br>Throws <see cref="ArgumentException"/> if records for any of the dates <paramref name="date1"/>, <paramref name="date2"/> don't exist</br>
        /// </summary>
        IList<StockPositionDiff> GetDiff(DateTime date1, DateTime date2);

        /// <summary>
        /// Checks if diff can be computed with this <paramref name="date"/>.
        /// </summary>
        bool DataExists(DateTime date);
    }
}
