using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Project.API.DataAccess;
using Project.DataAccess.Local.SQLServer;
using Project.DataAccess.Local.SQLServer.Models;
using Project.DataAccses.External.CSV;
using Project.DataAccses.External.CSV.Interfaces;

namespace Project.UI.Console
{
    public class DISetup
    {
        public IServiceProvider GetServiceProvider()
        {
            //https://rogerpence.dev/add-appsettings-json-file-to-a-c-console-app/
            //appsettings.json properties must be Copy Always

            //https://stackoverflow.com/questions/58530942/how-to-read-configuration-settings-before-initializing-a-host-in-asp-net-core
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

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
            });

            services.AddSingleton<IStockPositionsCSVParser, StockPositionsCSVParser>();
            services.AddSingleton<IStockPositionsLocalDAO, StockPositionsLocalDAO>();





            return services.BuildServiceProvider();
        }
    }
}
