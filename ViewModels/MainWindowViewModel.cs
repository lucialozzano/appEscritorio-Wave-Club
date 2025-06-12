using System.Windows.Input;
using WaveClubAppEscritorio2.Services;
using WaveClubAppEscritorio2.ViewModels.Activities;
using WaveClubAppEscritorio2.ViewModels.Bonds;
using WaveClubAppEscritorio2.ViewModels.Users;
using WaveClubAppEscritorio2.ViewModels.Bookings;
using WaveClubAppEscritorio2.ViewModels.UserBonds;
using WaveClubAppEscritorio2.ViewModels.Employees;
using WaveClubAppEscritorio2.ViewModels.Partners;
using WaveClubAppEscritorio2.Views;
using WaveClubAppEscritorio2.Views.Bookings;
using WaveClubAppEscritorio2.Views.UserBonds;

namespace WaveClubAppEscritorio2.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly APIClient _apiClient;
        private object _currentView;

        public MainWindowViewModel()
        {
            _apiClient = new APIClient();

            UsersViewModel = new UsersViewModel(_apiClient);
            ShowUsersViewCommand = new RelayCommand<object>(async _ => await ShowUsersView());

            ActivitiesViewModel = new ActivitiesViewModel(_apiClient);
            ShowActivitiesViewCommand = new RelayCommand<object>(async _ => await ShowActivitiesView());

            BondsViewModel = new BondsViewModel(_apiClient);
            ShowBondsViewCommand = new RelayCommand<object>(async _ => await ShowBondsView());

            BookingsViewModel = new BookingsViewModel(_apiClient);
            ShowBookingsViewCommand = new RelayCommand<object>(async _ => await ShowBookingsView());

            UserBondsViewModel = new UserBondsViewModel(_apiClient);
            ShowUserBondsViewCommand = new RelayCommand<object>(async _ => await ShowUserBondsView());

            EmployeesViewModel = new EmployeesViewModel(_apiClient);
            ShowEmployeesViewCommand = new RelayCommand<object>(async _ => await ShowEmployeesView());

            PartnersViewModel = new PartnersViewModel(_apiClient);
            ShowPartnersViewCommand = new RelayCommand<object>(async _ => await ShowPartnersView());
        }

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView)); 
            }
        }

        public UsersViewModel UsersViewModel { get; }
        public ICommand ShowUsersViewCommand { get; }

        private async Task ShowUsersView()
        {
            var usersView = new UserWindows(_apiClient);
            usersView.Show(); 
        }


        public ActivitiesViewModel ActivitiesViewModel { get;}
        public ICommand ShowActivitiesViewCommand { get; }

        private async Task ShowActivitiesView()
        {
            var activitiesView = new ActivityWindow(_apiClient);
            activitiesView.Show();
        }

        public BondsViewModel BondsViewModel { get; }
        public ICommand ShowBondsViewCommand { get; }
        private async Task ShowBondsView()
        {
            var bondsView = new BondWindow(_apiClient);
            bondsView.Show();
        }

        public BookingsViewModel BookingsViewModel { get; }
        public ICommand ShowBookingsViewCommand { get; }
        private async Task ShowBookingsView()
        {
            var bookingsView = new BookingWindow(_apiClient);
            bookingsView.Show();
        }

        public UserBondsViewModel UserBondsViewModel { get; }
        public ICommand ShowUserBondsViewCommand { get; }
        private async Task ShowUserBondsView()
        {
            var userBondsView = new UserBondWindows(_apiClient);
            userBondsView.Show();

        }

        public EmployeesViewModel EmployeesViewModel { get; }
        public ICommand ShowEmployeesViewCommand { get; }
        private async Task ShowEmployeesView()
        {
            var employeesView = new EmployeeWindows(_apiClient);
            employeesView.Show();
        }

        public PartnersViewModel PartnersViewModel { get; }
        public ICommand ShowPartnersViewCommand { get; }
        private async Task ShowPartnersView()
        {
            var partnersView = new PartnerWindows(_apiClient);
            partnersView.Show();
        }
    }
}
