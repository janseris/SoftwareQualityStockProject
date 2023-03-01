namespace Project.DataAccess.External.CSV.Interfaces
{
    public interface IStockPositionsCSVDAO
    {
        /// <summary>
        /// Loads records data in CSV format
        /// </summary>
        Task<byte[]> GetTodayRecordsCSV();
    }
}
