using Project.API.Models;
using Project.API.Services;

namespace Project.Logic.Services
{
    public class StockPositionDiffService : IStockPositionDiffService
    {
        public StockPositionDiff GetDiff(StockPositionRecord oldItem, StockPositionRecord newItem)
        {
            if (oldItem.Ticker != newItem.Ticker)
            {
                throw new ArgumentException($"{nameof(StockPositionDiff.Ticker)} must match.");
            }
            if (oldItem.Date.Date == newItem.Date.Date)
            {
                throw new ArgumentException($"Dates must differ.");
            }

            double sharesDiffPercent = GetSharesDiffPercent(oldItem.Shares, newItem.Shares);
            return new StockPositionDiff
            {
                CompanyName = newItem.CompanyName,
                Shares = newItem.Shares,
                SharesDiffPercent = sharesDiffPercent,
                Ticker = newItem.Ticker,
                WeightPercent = newItem.WeightPercent
            };
        }

        private double GetSharesDiffPercent(int oldShares, int newShares)
        {
            int sharesDiff = newShares - oldShares;
            double sharesDiffPercent = sharesDiff / 100d;
            return sharesDiffPercent;
        }

        private bool ContainsDuplicatesByTicker(IList<StockPositionRecord> items)
        {
            return items.DistinctBy(item => item.Ticker).Count() != items.Count;
        }

        public IList<StockPositionDiff> GetDiff(IList<StockPositionRecord> positions1, IList<StockPositionRecord> positions2)
        {
            Validate(positions1, positions2);
            var positions1Dictionary = positions1.ToDictionary(item => item.Ticker, item => item);
            var positions2Dictionary = positions2.ToDictionary(item => item.Ticker, item => item);

            var result = new List<StockPositionDiff>();

            //get "new positions"
            var newPositions = GetNewPositions(positions1Dictionary, positions2Dictionary);
            result.AddRange(newPositions);

            //get diffs

            var diffs = GetDiffs(positions1Dictionary, positions2Dictionary);
            result.AddRange(diffs);

            return result;
        }

        private IList<StockPositionDiff> GetNewPositions(Dictionary<string, StockPositionRecord> positions1Dictionary, Dictionary<string, StockPositionRecord> positions2Dictionary)
        {
            var result = new List<StockPositionDiff>();

            var newPositionTickers = positions2Dictionary.Keys.Except(positions1Dictionary.Keys).ToList();
            foreach (var newPositionTicker in newPositionTickers)
            {
                var item = positions2Dictionary[newPositionTicker];
                result.Add(new StockPositionDiff
                {
                    CompanyName = item.CompanyName,
                    Shares = item.Shares,
                    SharesDiffPercent = null,
                    Ticker = item.Ticker,
                    WeightPercent = item.WeightPercent
                });
            }
            return result;
        }

        private IList<StockPositionDiff> GetDiffs(Dictionary<string, StockPositionRecord> positions1Dictionary, Dictionary<string, StockPositionRecord> positions2Dictionary)
        {
            var result = new List<StockPositionDiff>();

            foreach (var kvp in positions2Dictionary)
            {
                string ticker = kvp.Key;
                StockPositionRecord position2 = kvp.Value;
                if (positions1Dictionary.ContainsKey(ticker) == false)
                {
                    continue; //skip - this is a new position
                }
                var position1 = positions1Dictionary[ticker];
                var diff = GetDiff(position1, position2);
                result.Add(diff);
            }
            return result;
        }




        private void Validate(IList<StockPositionRecord> items1, IList<StockPositionRecord> items2)
        {
            ThrowIfContainsDuplicates(items1, "first");
            ThrowIfContainsDuplicates(items2, "second");
            ThrowIfContainsVariousDates(items1);
            ThrowIfContainsVariousDates(items2);
        }

        private void ThrowIfContainsDuplicates(IList<StockPositionRecord> items, string listName)
        {
            if (ContainsDuplicatesByTicker(items))
            {
                throw new ArgumentException($"The {listName} list contains multiple entries for one company (invalid)!");
            }
        }

        private void ThrowIfContainsVariousDates(IList<StockPositionRecord> items)
        {
            if (items.Count == 0)
            {
                return;
            }
            var date = items.First().Date.Date;
            foreach (var item in items)
            {
                if (item.Date.Date != date)
                {
                    throw new ArgumentException("Dates in all items must match.");
                }
            }
        }
    }
}
