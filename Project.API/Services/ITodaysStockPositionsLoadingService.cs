using System.Collections.Generic;
using System.Threading.Tasks;

using Project.API.Models;

namespace Project.API.Services
{
    /// <summary>
    /// Obtains data from external data source
    /// </summary>
    public interface ITodaysStockPositionsLoadingService
    {
        /// <summary>
        /// Returns records for the current day.
        /// </summary>
        /// <returns></returns>
        Task<IList<StockPositionRecord>> GetTodaysRecords();
    }
}
