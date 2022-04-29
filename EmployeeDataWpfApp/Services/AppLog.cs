using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDataWpfApp.Services
{
    public class AppLog
    {
        public void Logger()
        {
            var logger = LogManager.GetCurrentClassLogger();

            try
            {
                var config = new ConfigurationBuilder()
                   .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                   .Build();


                //using var servicesProvider_DepWithMaxSalary = new ServiceCollection()
                //    .AddTransient<DepartmentWithMaxSalary>()
                //    .AddLogging(loggingBuilder =>
                //    {
                //        loggingBuilder.ClearProviders();
                //        loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Debug);
                //        loggingBuilder.AddNLog(config);
                //    }).BuildServiceProvider();

                //var depWithMaxSalary = servicesProvider_DepWithMaxSalary.GetRequiredService<DepartmentWithMaxSalary>();
                //depWithMaxSalary.GetDepartmentWithMaxSalary();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }
    }
}
