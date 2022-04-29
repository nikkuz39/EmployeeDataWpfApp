using EmployeeDataWpfApp.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EmployeeDataWpfApp.ViewModels
{
    public class StatusStatisticsViewModel : ObservableObj
    {
        private String GetDatabaseConnectionString()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();

            return configuration.GetConnectionString("DefaultConnection");
        }

        private List<Status> statusList;
        public List<Status> StatusList
        {
            get { return statusList; }
            set
            {
                statusList = value;
                OnPropertyChanged("StatusList");
            }
        }

        private IEnumerable<Person> statistics;
        public IEnumerable<Person> Statistics
        {
            get { return statistics; }
            set
            {
                statistics = value;
                OnPropertyChanged("Statistics");
            }
        }

        private Status statusId = new Status { Id = 0 };
        public Status StatusComboBoxSelectItem
        {
            get { return statusId; }
            set
            {
                statusId = value;
                GetEmployeeStatistics();
                OnPropertyChanged("StatusComboBoxSelectItem");                               
            }
        }

        private DateTime startDate = DateTime.Now;
        public DateTime StartDatePickerSelectedDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                GetEmployeeStatistics();
                OnPropertyChanged("StartDatePickerSelectedDate");
            }
        }

        private DateTime endDate = DateTime.Now;
        public DateTime EndDatePickerSelectedDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
                GetEmployeeStatistics();
                OnPropertyChanged("EndDatePickerSelectedDate");
            }
        }

        public StatusStatisticsViewModel()
        {
            GetStatusList();            
        }

        private void GetStatusList()
        {
            using (CompanyContext context = new CompanyContext())
            {
                StatusList = context.Statuses.AsNoTracking().ToList();                
            }
        }

        private void GetEmployeeStatistics()
        {
            if (statusId == null) return;

            switch (statusId.Id)
            {
                case 1:
                    Statistics = LoadEmploymentStatistics(startDate, endDate);
                    break;
                case 2:
                    Statistics = LoadDismissStatistics(startDate, endDate);
                    break;
            }
        }

        private IEnumerable<Person> LoadEmploymentStatistics(DateTime startDate, DateTime endDate)
        {
            string storedProcedureNameInSql = "EmploymentStatistics";

            using (SqlConnection connection = new SqlConnection(GetDatabaseConnectionString()))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(storedProcedureNameInSql, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    SqlParameter startDateParameter = new SqlParameter
                    {
                        ParameterName = "@startDate",
                        Value = startDate
                    };

                    command.Parameters.Add(startDateParameter);

                    SqlParameter endDateParameter = new SqlParameter
                    {
                        ParameterName = "@endDate",
                        Value = endDate
                    };

                    command.Parameters.Add(endDateParameter);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            Person person = new Person();

                            person.Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                            person.DateEmploy = reader.IsDBNull(1) ? null : reader.GetDateTime(1);

                            yield return person;
                        }
                        reader.Close();
                    }
                }
            }
        }

        private IEnumerable<Person> LoadDismissStatistics(DateTime startDate, DateTime endDate)
        {
            string storedProcedureNameInSql = "DismissStatistics";

            using (SqlConnection connection = new SqlConnection(GetDatabaseConnectionString()))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(storedProcedureNameInSql, connection);

                command.CommandType = CommandType.StoredProcedure;

                SqlParameter startDateParameter = new SqlParameter
                {
                    ParameterName = "@startDate",
                    Value = startDate
                };

                command.Parameters.Add(startDateParameter);

                SqlParameter endDateParameter = new SqlParameter
                {
                    ParameterName = "@endDate",
                    Value = endDate
                };

                command.Parameters.Add(endDateParameter);

                SqlDataReader reader = command.ExecuteReader();

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        Person person = new Person();

                        person.Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                        person.DateUnemploy = reader.IsDBNull(1) ? null : reader.GetDateTime(1);

                        yield return person;
                    }
                }
            }
        }
    }
}
