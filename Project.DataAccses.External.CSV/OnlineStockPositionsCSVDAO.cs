using Project.DataAccses.External.CSV.Interfaces;

namespace Project.DataAccses.External.CSV
{
    /// <summary>
    /// Retrieves CSV with stock positions from an online source
    /// </summary>
    public class OnlineStockPositionsCSVDAO : IStockPositionsCSVDAO
    {
        private readonly HttpClient httpClient;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpClient">with base address (URL) pointing to the requested resource</param>
        public OnlineStockPositionsCSVDAO(HttpClient httpClient)
        {
            if(httpClient.BaseAddress is null)
            {
                throw new ArgumentException($"{nameof(httpClient.BaseAddress)} must be set");
            }
            this.httpClient = httpClient;
        }

        public async Task<byte[]> GetCSV()
        {
            return await GetCSV(httpClient.BaseAddress! /* "!" = assert not null */);
        }

        private async Task<byte[]> GetCSV(Uri url)
        {
            return await httpClient.GetByteArrayAsync(url);
        }
    }
}
