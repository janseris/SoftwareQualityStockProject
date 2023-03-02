namespace Project.API.Models
{
    /// <summary>
    /// The result to be displayed as info to the user
    /// </summary>
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

        public int Shares { get; set; }

        public double WeightPercent { get; set; }

        public override string ToString()
        {
            return $"{CompanyName} ({Ticker}), shares: {Shares.ToString("#,##0")} {PrintSharesDiffPercent()}, weight: {WeightPercent.ToString("N2")} %";
        }

        private string PrintSharesDiffPercent()
        {
            if(SharesDiffPercent == null)
            {
                return string.Empty;
            }
            return $"({SharesDiffPercent.Value.ToString("+#.##;-#.##;0")} %)";
        }
    }
}
