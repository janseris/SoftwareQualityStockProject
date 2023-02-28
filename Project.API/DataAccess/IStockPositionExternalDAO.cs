﻿using System.Collections.Generic;

using Project.API.Models;

namespace Project.API.DataAccess
{
    /// <summary>
    /// Obtains data from external data source
    /// </summary>
    public interface IStockPositionExternalDAO
    {
        /// <summary>
        /// Returns records for the current day.
        /// </summary>
        /// <returns></returns>
        IList<StockPositionRecord> GetAll();
    }
}
