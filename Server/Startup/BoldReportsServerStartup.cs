using BoldReports.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oqtane.Infrastructure;
using System.Collections.Generic;

namespace SPSITECH.Module.BoldReports.Startup
{
    public class BoldReportsServerStartup : IServerStartup
    {

        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false,
                reloadOnChange: true);

            // BOLD REPORTS:  ???
            //app.UseEndpoints(endpoints =>
           //{
            //    endpoints.MapControllers();
            //    endpoints.MapBlazorHub();
           //     endpoints.MapFallbackToPage("/_Host");
           // });

            var Configuration = builder.Build();
           
            var BoldReportsLicense = Configuration.GetSection("BoldReportsLicense");
            if (BoldReportsLicense != null)
            {
                Bold.Licensing.BoldLicenseProvider.RegisterLicense(BoldReportsLicense.Value);
                ReportConfig.DefaultSettings = new ReportSettings().RegisterExtensions(new List<string> { "BoldReports.Data.WebData", "BoldReports.Data.MySQL", "BoldReports.Data.Csv" });
            }
            

        }

        public void ConfigureMvc(IMvcBuilder mvcBuilder)
        {
            // not implemented
        }

        public void ConfigureServices(IServiceCollection services)
        {
            
        }
    }
}
