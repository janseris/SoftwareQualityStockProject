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

            string sampleDataFolderPath = "../../../../sampleData";

            await ImportDataToDB(provider, new DateTime(2023, 2, 28), "28feb2023 - ARK_INNOVATION_ETF_ARKK_HOLDINGS.csv", sampleDataFolderPath);
            Console.WriteLine();
            await ImportDataToDB(provider, new DateTime(2023, 3, 1), "1mar2023 - ARK_INNOVATION_ETF_ARKK_HOLDINGS.csv", sampleDataFolderPath);
            Console.WriteLine();

            Console.ReadKey(); //do not exit immediately
        }

        private static async Task<bool> ImportDataToDB(IServiceProvider provider, DateTime date, string csvFileName, string sampleDataFolderPath, bool overwriteIfExists = false)
        {
            var csvFilePath = Path.Combine(sampleDataFolderPath, csvFileName);
            var imported = await ImportDataToDBInternal(provider, date, csvFilePath, overwriteIfExists);
            if (imported)
            {
                Console.WriteLine($"Data for {date.ToString("d. MMM. yyyy")} imported.");
            }
            return imported;
        }

        /// <summary>
        /// Code taken from <see cref="StockPositionsAdministrationFacade.SaveTodaysStockPositions"/>
        /// </summary>
        private static async Task<bool> ImportDataToDBInternal(IServiceProvider provider, DateTime date, string csvFilePath, bool overwriteIfExists = false)
        {
            var service = provider.GetRequiredService<ITodaysStockPositionsLoadingService>() as TodaysStockPositionsLoadingService;

            var inputDataBytes = File.ReadAllBytes(csvFilePath);
            var data = service.GetTodaysRecordsFromCSVFile(inputDataBytes);

            var localDAO = provider.GetRequiredService<IStockPositionsLocalDAO>();


            //the code from StockPositionsAdministrationFacade#SaveTodaysStockPositions
            var existsForTheDate = await localDAO.AnyRecordsExist(date);

            if (existsForTheDate && overwriteIfExists)
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