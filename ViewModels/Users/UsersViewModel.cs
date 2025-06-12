using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WaveClubAppEscritorio2.Models;
using WaveClubAppEscritorio2.Services;
using WaveClubAppEscritorio2.Views;

namespace WaveClubAppEscritorio2.ViewModels.Users
{
    public class UsersViewModel : ViewModelBase
    {
        private readonly APIClient _apiClient;

        public UsersViewModel(APIClient apiClient)
        {
            NavigateToEditUserCommand = new RelayCommand<object>(async _ => await NavigateToEditUser());
            _apiClient = apiClient;
            LoadUsersCommand = new RelayCommand<object>(async _ => await LoadUsersAsync());
            DeleteUserCommand = new RelayCommand<int>(async userId => await DeleteUserAsync(userId));
            EditUserCommand = new RelayCommand<int>(async userId => await EditUserAsync(userId));
            Users = new ObservableCollection<User>();

            _ = LoadUsersAsync();
        }

        public ObservableCollection<User> Users { get; }

        public ICommand LoadUsersCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand EditUserCommand { get; }
        public ICommand NavigateToEditUserCommand { get; private set; }


        public async Task LoadUsersAsync()
        {
            var users = await _apiClient.GetUsersAsync();
            Users.Clear();
            foreach (var user in users)
            {
                Users.Add(user);
            }
        }

        private async Task DeleteUserAsync(int userId)
        {
            var success = await _apiClient.DeleteUserAsync(userId);
            if (success)
            {
                var user = Users.FirstOrDefault(u => u.Id == userId);
                if (user != null)
                {
                    Users.Remove(user);
                }
            }
        }

        private async Task NavigateToEditUser()
        {
            var newUser = new User();
            var editUserWindow = new EditUserWindow(newUser, _apiClient, true, this);

            if (editUserWindow.ShowDialog() == true)
            {
                _ = LoadUsersAsync();
            }
        }

        private async Task EditUserAsync(int userId)
        {
            var user = Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                var editWindow = new EditUserWindow(user, _apiClient, false, this); 
                if (editWindow.ShowDialog() == true)
                {
                    await LoadUsersAsync();
                }
            }
        }

        public async Task AddUserAsync(User user)
        {
            var createdUser = await _apiClient.CreateUserAsync(user);
            if (createdUser)
            {
                Users.Add(user);  
            }
            else
            {
                MessageBox.Show("Error al guardar el usuario. Intenta nuevamente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
