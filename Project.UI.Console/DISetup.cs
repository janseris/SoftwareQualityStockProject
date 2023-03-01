using Microsoft.Extensions.DependencyInjection;

using Project.DataAccses.External.CSV;
using Project.DataAccses.External.CSV.Interfaces;

namespace Project.UI.Console
{
    public class DISetup
    {
        public IServiceProvider GetServiceProvider(string csvURL)
        {
            ServiceCollection services = new ServiceCollection();

            //this 1 call does 2 things: 
            // 1. registers OnlineOSMImageMapTileDataProvider with transient lifetime 
            // 2. adds HttpClient to it (these HTTPClients' lifetime is managed by ASP.NET)
            //https://stackoverflow.com/questions/65777953/why-httpclient-does-not-hold-the-base-address-even-when-its-set-in-startup
            //https://stackoverflow.com/questions/59280153/dependency-injection-httpclient-or-httpclientfactory
            services.AddHttpClient<IStockPositionsCSVDAO, OnlineStockPositionsCSVInternalDAO>(httpClient =>
            {
                httpClient.BaseAddress = new Uri(csvURL);
            });


        }
    }
}
