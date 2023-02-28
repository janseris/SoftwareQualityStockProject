using System;

namespace Project.API.Models
{
    /// <summary>
    /// Represents a status of one company (<see cref="Ticker"/>, <see cref="CompanyName"/>) in a day <see cref="Date"/>
    /// </summary>
    public class StockPositionRecord
    {
        /// <summary>
        /// Only years, months and days are used.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Informative attribute
        /// <br>Not <c>null</c></br>
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Shortened company name for telegraph transmissions
        /// <br>Serves as a unique identifier of a company</br>
        /// <br>Not <c>null</c> and not empty</br>
        /// </summary>
        public string Ticker { get; set; }

        /// <summary>
        /// Not negative
        /// </summary>
        public int Shares { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date">only date part is preserved (time part is stripped)</param>
        /// <param name="companyName">any non-null string</param>
        /// <param name="ticker">not null and not empty or whitespace</param>
        /// <param name="shares">non-negative</param>
        /// <exception cref="ArgumentException"></exception>
        public StockPositionRecord(DateTime date, string companyName, string ticker, int shares)
        {
            #region validity checks
            if (shares <= 0)
            {
                throw new ArgumentException($"{nameof(shares)} cannot be negative.");
            }
            if(companyName is null)
            {
                throw new ArgumentException($"{nameof(companyName)} cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(ticker))
            {
                throw new ArgumentException($"{nameof(ticker)} cannot be null or empty (or whitespace).");
            }
            #endregion

            Date = date;
            CompanyName = companyName;
            Ticker = ticker;
            Shares = shares;
        }
    }
}
