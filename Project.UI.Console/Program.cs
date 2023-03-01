﻿using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
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

            bool todaySaved = await facade.AreTodaysStockPositionsAlreadySaved();

            Console.WriteLine("Todays data aren't already saved in the DB.");

            Console.WriteLine("Retrieving and saving today's data.");
            await facade.SaveTodaysStockPositions(overwrite: true);

            Console.WriteLine("Todays data aren't already saved in DB.");

            var availableDates = await facade.GetAvailableDates();
            Console.WriteLine("Available dates:");
            foreach (var date in availableDates)
            {
                Console.WriteLine(date.ToString("d. M. yyyy"));
            }

            if (availableDates.Count >= 2)
            {
                var firstDate = availableDates[0];
                var secondDate = availableDates[1];
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
            var changedPositions = (from item in stockPositionDiffs where item.SharesDiffPercent is not null select item).ToList();
            foreach (var position in changedPositions)
            {
                Console.WriteLine(position);
            }
        }
    }
}