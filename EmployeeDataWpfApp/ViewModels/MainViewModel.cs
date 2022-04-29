using EmployeeDataWpfApp.Services;
using EmployeeDataWpfApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EmployeeDataWpfApp.ViewModels
{
    public class MainViewModel : ObservableObj
    {
        public RelayCommand ListOfEmployeesViewCommand { get; set; }
        public ListOfEmployeesViewModel ListOfEmployeesVM { get; set; }

        public RelayCommand StatusStatisticsViewCommand { get; set; }
        public StatusStatisticsViewModel StatusStatisticsVM { get; set; }


        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            ListOfEmployeesVM = new ListOfEmployeesViewModel();

            ListOfEmployeesViewCommand = new RelayCommand(o =>
            {
                CurrentView = ListOfEmployeesVM;
            });

            StatusStatisticsVM = new StatusStatisticsViewModel();
            StatusStatisticsViewCommand = new RelayCommand(o =>
           {
               CurrentView = StatusStatisticsVM;
           });
        }

        private RelayCommand closeMainWindow;
        public RelayCommand CloseMainWindow
        {
            get
            {
                return closeMainWindow ??
                    (closeMainWindow = new RelayCommand(o =>
                    {
                        Application.Current.MainWindow.Close();
                    }));
            }
        }
    }
}
