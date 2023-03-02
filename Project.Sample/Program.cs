using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.DependencyInjection;

using Project.API.Facades;
using Project.API.Models;

namespace Project.Sample
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var provider = DISetup.GetServiceProvider();
            var facade = provider.GetRequiredService<IStockPositionsAdministrationFacade>();
            var diffFacade = provider.GetRequiredService<IStockPositionsDiffFacade>();



            ////doesnt work well if date in csv does not match local date because of timezone
            //bool todaySaved = await facade.AreTodaysStockPositionsAlreadySaved();

            //var verb = todaySaved ? "are" : "aren't";
            //Console.WriteLine($"Todays data {verb} already saved in the DB.");

            //var conditionalOverwriteText = todaySaved ? " (overwriting)" : "";
            //Console.WriteLine($"Retrieving and saving today's data{conditionalOverwriteText}.");
            //await facade.SaveTodaysStockPositions(overwrite: true);



            var availableDates = await facade.GetAvailableDates();
            Console.WriteLine("Available dates in DB:");
            foreach (var date in availableDates)
            {
                Console.WriteLine(date.ToString("d. MMM. yyyy"));
            }
            Console.WriteLine();

            if (availableDates.Count >= 2)
            {
                var firstDate = availableDates[0];
                var secondDate = availableDates[1];

                Console.WriteLine($"Comparning dates {firstDate.ToString("d. MMM. yyyy")} and {secondDate.ToString("d. MMM. yyyy")}");
                var diff = await diffFacade.GetDiff(firstDate, secondDate);
                PrintDiff(diff);
            }
        }

        private static void PrintDiff(IList<StockPositionDiff> stockPositionDiffs)
        {
            var newPositions = (from item in stockPositionDiffs where item.SharesDiffPercent is null select item).ToList();
            Console.WriteLine("New positions:");
            foreach(var position in newPositions)
            {
                Console.WriteLine(position);
            }
            
            Console.WriteLine();

            var changedPositions = (from item in stockPositionDiffs 
                                    where item.SharesDiffPercent is not null 
                                    select item).ToList();

            var increasedPositions = (from item in changedPositions
                                    where item.SharesDiffPercent!.Value >= 0
                                    select item).ToList();

            var decreasedPositions = (from item in changedPositions
                                      where item.SharesDiffPercent!.Value < 0
                                      select item).ToList();

            Console.WriteLine("Increased positions:");
            foreach (var position in increasedPositions)
            {
                Console.WriteLine(position);
            }

            Console.WriteLine();

            Console.WriteLine("Decreased positions:");
            foreach (var position in decreasedPositions)
            {
                Console.WriteLine(position);
            }
        }
    }
}