namespace Project.API.Models
{
    public class StockPositionDiff
    {
        public string CompanyName { get; set; }

        /// <summary>
        /// Shortened company name for telegraph transmissions
        /// </summary>
        public string Ticker { get; set; }

        /// <summary>
        /// <c>null => new company</c>
        /// </summary>
        public double? SharesDiffPercent { get; set; }
    }
}
