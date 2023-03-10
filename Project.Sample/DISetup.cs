using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Project.API.DataAccess;
using Project.API.Facades;
using Project.API.Services;
using Project.DataAccess.External.CSV;
using Project.DataAccess.External.CSV.Interfaces;
using Project.DataAccess.Local.SQLServer;
using Project.DataAccess.Local.SQLServer.Models;
using Project.Logic.Facades;
using Project.Logic.Services;

namespace Project.Sample
{
    public class DISetup
    {
        private const string DefaultFirefoxUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:105.0) Gecko/20100101 Firefox/105.0";

        public static IServiceProvider GetServiceProvider()
        {
            //https://rogerpence.dev/add-appsettings-json-file-to-a-c-console-app/
            //appsettings.json was added menually to the project because it is not an ASP.NET application and in Properties, it must have "Copy Always"

            //load appsettings.json, source: https://stackoverflow.com/questions/58530942/how-to-read-configuration-settings-before-initializing-a-host-in-asp-net-core
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            //read from appsettings.json
            var connectionString = configuration.GetConnectionString("Local");
            string csvURL = configuration.GetSection("CSVHttpClientConfig")["URL"];

            ServiceCollection services = new ServiceCollection();

            services.AddSingleton<IConfiguration>(configuration);

            services.AddDbContextFactory<SWQualityProjectContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            //this 1 call does 2 things: 
            // 1. registers OnlineOSMImageMapTileDataProvider with transient lifetime 
            // 2. adds HttpClient to it (these HTTPClients' lifetime is managed by ASP.NET)
            //https://stackoverflow.com/questions/65777953/why-httpclient-does-not-hold-the-base-address-even-when-its-set-in-startup
            //https://stackoverflow.com/questions/59280153/dependency-injection-httpclient-or-httpclientfactory
            services.AddHttpClient<IStockPositionsCSVDAO, OnlineStockPositionsCSVDAO>(httpClient =>
            {
                httpClient.BaseAddress = new Uri(csvURL);
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(DefaultFirefoxUserAgent);
            });

            services.AddSingleton<IStockPositionsCSVParser, StockPositionsCSVParser>();
            services.AddSingleton<IStockPositionsLocalDAO, StockPositionsLocalDAO>();
            services.AddSingleton<StockPositionInsertDBMapper>();
            services.AddSingleton<ITodaysStockPositionsLoadingService, TodaysStockPositionsLoadingService>();
            services.AddSingleton<IStockPositionsAdministrationFacade, StockPositionsAdministrationFacade>();

            services.AddSingleton<IStockPositionDiffService, StockPositionDiffService>();
            services.AddSingleton<IStockPositionsDiffFacade, StockPositionsDiffFacade>();


            //logging
            //source: https://thecodeblogger.com/2021/05/11/how-to-enable-logging-in-net-console-applications/
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("Project.Sample", LogLevel.Debug)
                    .AddConsole();
            });
            services.AddSingleton(loggerFactory.CreateLogger<StockPositionsCSVParser>());
                                  
            return services.BuildServiceProvider(); 
        } 
    } 
}           
