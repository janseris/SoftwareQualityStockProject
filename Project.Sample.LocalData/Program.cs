using Microsoft.Extensions.DependencyInjection;

using Project.API.DataAccess;
using Project.API.Services;
using Project.Logic.Facades;
using Project.Logic.Services;

namespace Project.Sample.LocalData
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var provider = DISetup.GetServiceProvider();
            var service = provider.GetRequiredService<ITodaysStockPositionsLoadingService>() as TodaysStockPositionsLoadingService;

            var date = new DateTime(2023, 2, 28);
            var csvFilePath = "../../../../sampleData/28feb2023 - ARK_INNOVATION_ETF_ARKK_HOLDINGS.csv";
            var imported = await ImportDataToDB(provider, date, csvFilePath, overWriteIfExists: true);
            if (imported)
            {
                Console.WriteLine("Data imported.");
            }
        }

        /// <summary>
        /// Code taken from <see cref="StockPositionsAdministrationFacade.SaveTodaysStockPositions"/>
        /// </summary>
        private static async Task<bool> ImportDataToDB(IServiceProvider provider, DateTime date, string csvFilePath, bool overWriteIfExists = false)
        {
            var service = provider.GetRequiredService<ITodaysStockPositionsLoadingService>() as TodaysStockPositionsLoadingService;

            var inputDataBytes = File.ReadAllBytes(csvFilePath);
            var data = service.GetTodaysRecordsFromCSVFile(inputDataBytes);

            var localDAO = provider.GetRequiredService<IStockPositionsLocalDAO>();


            //the code from StockPositionsAdministrationFacade#SaveTodaysStockPositions
            var existsForTheDate = await localDAO.AnyRecordsExist(date);

            if (existsForTheDate && overWriteIfExists)
            {
                //TODO: transaction
                await localDAO.DeleteForDate(date);
                await localDAO.InsertAll(data);
                Console.WriteLine($"Data for the date {date.ToString("d. MMM. yyyy")} already existed and was overwritten.");
                return true;
            }
            if (existsForTheDate == false)
            {
                await localDAO.InsertAll(data);
                Console.WriteLine($"Data for date {date.ToString("d. MMM. yyyy")} was saved into the DB.");
                return true;
            }
            Console.WriteLine($"Data for the date {date.ToString("d. MMM. yyyy")} already existed. No-op.");
            return false;
        }
    }
}