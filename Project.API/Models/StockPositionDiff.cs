﻿namespace Project.API.Models
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
    }
}
