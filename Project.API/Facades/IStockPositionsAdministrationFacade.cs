using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Project.API.Models;

namespace Project.API.Facades
{
    /// <summary>
    /// Used by an administrator end-user of the server process and/or automaatically by the process itself.
    /// </summary>
    public interface IStockPositionsAdministrationFacade
    {
        /// <summary>
        /// Saves today's stock position records from the web to local persistent storage for future diff calculations
        /// </summary>
        /// <param name="overwrite">specifies behavior on when the info has been already saved to the local persistent storage</param>
        /// <returns><c>true</c> if data has been written, <c>false</c> if not</returns>
        Task<bool> SaveTodaysStockPositions(bool overwrite);

        /// <summary>
        /// Checks whether the info has been scraped for today from the web and is saved in the persistent local storage.
        /// <br>Warning: "today" on local machine might differ (+1/-1) from the date in the received data from the remote server</br>
        /// </summary>
        Task<bool> AreTodaysStockPositionsAlreadySaved();

        /// <summary>
        /// Checks whether the info has been scraped for <see cref="date"/> from the web and is saved in the persistent local storage.
        /// </summary>
        Task<bool> AreStockPositionsAlreadySaved(DateTime date);

        /// <summary>
        /// Returns a list of dates which can be used for diffs (there are some records stored in the persistent local storage for them)
        /// </summary>
        Task<IList<DateTime>> GetAvailableDates();

        /// <summary>
        /// Returns all stock position records for date <paramref name="date"/> saved in the persistent local storage which can be used for diff calculations
        /// </summary>
        Task<IList<StockPositionRecord>> GetAll(DateTime date);
    }
}
