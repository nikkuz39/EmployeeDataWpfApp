using EmployeeDataWpfApp.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace EmployeeDataWpfApp.ViewModels
{
    public class ListOfEmployeesViewModel : ObservableObj
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private String GetDatabaseConnectionString()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();

            return configuration.GetConnectionString("DefaultConnection");
        }       

        private List<Status>? statusList;
        public List<Status>? StatusList
        {
            get { return statusList; }
            set
            {
                statusList = value;
                OnPropertyChanged("StatusList");
            }
        }

        private List<Dep>? departmentList;
        public List<Dep>? DepartmentList
        {
            get { return departmentList; }
            set
            {
                departmentList = value;
                OnPropertyChanged("DepartmentList");
            }
        }

        private List<Post>? postList;
        public List<Post>? PostList
        {
            get { return postList; }
            set
            {
                postList = value;
                OnPropertyChanged("PostList");
            }
        }

        private IEnumerable<Person>? persons;
        public IEnumerable<Person>? Persons
        {
            get { return persons; }
            set
            {
                persons = value;
                OnPropertyChanged("Persons");
            }
        }        

        private int statusId = 0;
        public Status StatusComboBoxSelectItem
        {
            set
            {
                statusId = value.Id;
                Persons = LoadAllPersons(statusId, departmentId, postId, searchLastName);
                OnPropertyChanged("StatusComboBoxSelectItem");
            }
        }

        private int departmentId = 0;
        public Dep DepartmentComboBoxSelectItem
        {
            set
            {
                departmentId = value.Id;
                Persons = LoadAllPersons(statusId, departmentId, postId, searchLastName);
                OnPropertyChanged("DepartmentComboBoxSelectItem");
            }
        }

        private int postId = 0;
        public Post PostComboBoxSelectItem
        {
            set
            {
                postId = value.Id;
                Persons = LoadAllPersons(statusId, departmentId, postId, searchLastName);
                OnPropertyChanged("PostComboBoxSelectItem");
            }
        }

        private string searchLastName = "";
        public string SearchLastName
        {
            set
            {
                searchLastName = value;
                Persons = LoadAllPersons(statusId, departmentId, postId, searchLastName);
                OnPropertyChanged("SearchLastName");
            }
        }

        public ListOfEmployeesViewModel()
        {
            GetStatusList();
            GetDepartmentList();
            GetPostList();
        }
        

        private void GetStatusList()
        {
            using (CompanyContext context = new CompanyContext())
            {
                try
                {
                    logger.Info("Call Method 'GetStatusList' / class 'ListOfEmployeesViewModel'");

                    StatusList = context.Statuses.AsNoTracking().ToList();
                    StatusList.Insert(0, new Status { Id = 0, Name = "All" });
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Method 'GetStatusList' / class 'ListOfEmployeesViewModel'");
                }
            }
        }

        private void GetDepartmentList()
        {
            using (CompanyContext context = new CompanyContext())
            {
                try
                {
                    logger.Info("Call Method 'GetDepartmentList' / class 'ListOfEmployeesViewModel'");

                    DepartmentList = context.Deps.AsNoTracking().ToList();
                    DepartmentList.Insert(0, new Dep { Id = 0, Name = "All" });
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Method 'GetDepartmentList' / class 'ListOfEmployeesViewModel'");
                }
            }
        }

        private void GetPostList()
        {
            using (CompanyContext context = new CompanyContext())
            {
                try
                {
                    logger.Info("Call Method 'GetPostList' / class 'ListOfEmployeesViewModel'");

                    PostList = context.Posts.AsNoTracking().ToList();
                    PostList.Insert(0, new Post { Id = 0, Name = "All" });
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Method 'GetPostList' / class 'ListOfEmployeesViewModel'");
                }
            }
        }

        private IEnumerable<Person> LoadAllPersons(int statusId, int depId, int postId, string lastName)
        {
            string storedProcedureNameInSql = "EmployeesSearch";

            using (SqlConnection connection = new SqlConnection(GetDatabaseConnectionString()))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(storedProcedureNameInSql, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    SqlParameter statusNumberParameter = new SqlParameter
                    {
                        ParameterName = "@statusNumbers",
                        Value = (statusId == 0) ? null : statusId
                    };

                    command.Parameters.Add(statusNumberParameter);

                    SqlParameter departmentNumberParameter = new SqlParameter
                    {
                        ParameterName = "@departmentNumbers",
                        Value = (depId == 0) ? null : depId
                    };

                    command.Parameters.Add(departmentNumberParameter);

                    SqlParameter postNumberParameter = new SqlParameter
                    {
                        ParameterName = "@postNumber",
                        Value = (postId == 0) ? null : postId
                    };

                    command.Parameters.Add(postNumberParameter);

                    SqlParameter lastNameParameter = new SqlParameter
                    {
                        ParameterName = "@lastName",
                        Value = $"%{lastName}%"
                    };

                    command.Parameters.Add(lastNameParameter);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            Person person = new Person();
                            Status status = new Status();
                            Dep dep = new Dep();
                            Post post = new Post();

                            person.Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);

                            person.LastName = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            person.FirstName = reader.IsDBNull(2) ? "" : reader.GetString(2);
                            person.SecondName = reader.IsDBNull(3) ? "" : reader.GetString(3);
                            status.Id = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
                            status.Name = reader.IsDBNull(5) ? "" : reader.GetString(5);
                            dep.Id = reader.IsDBNull(6) ? 0 : reader.GetInt32(6);
                            dep.Name = reader.IsDBNull(7) ? "" : reader.GetString(7);
                            post.Id = reader.IsDBNull(8) ? 0 : reader.GetInt32(8);
                            post.Name = reader.IsDBNull(9) ? "" : reader.GetString(9);
                            person.DateEmploy = reader.IsDBNull(10) ? null : reader.GetDateTime(10);
                            person.DateUnemploy = reader.IsDBNull(11) ? null : reader.GetDateTime(11);

                            person.Status = status;
                            person.Dep = dep;
                            person.Post = post;

                            yield return person;
                        }
                        reader.Close();
                    }                    
                }
            }
        }
    }
}
