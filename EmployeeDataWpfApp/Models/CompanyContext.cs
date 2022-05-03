using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EmployeeDataWpfApp
{
    public class CompanyContext : DbContext
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public DbSet<Dep>? Deps { get; set; }
        public DbSet<Person>? Persons { get; set; }
        public DbSet<Post>? Posts { get; set; }
        public DbSet<Status>? Statuses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            try
            {
                optionsBuilder.LogTo(message => logger.Debug(message, "Method 'OnConfiguring' / class 'CompanyContext'"))
                    .UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    options => options.EnableRetryOnFailure
                    (
                        maxRetryCount: 2,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorNumbersToAdd: null)
                    );
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Method 'OnConfiguring' / class 'CompanyContext'");
            }
        }
    }
}
