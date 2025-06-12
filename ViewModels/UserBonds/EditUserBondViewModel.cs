using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using WaveClubAppEscritorio2.Models;
using WaveClubAppEscritorio2.Services;

namespace WaveClubAppEscritorio2.ViewModels.UserBonds
{
    public class EditUserBondViewModel : ViewModelBase
    {
        private readonly APIClient _apiClient;
        public UserBond UserBond { get; private set; }

        public ObservableCollection<User> Users { get; private set; }
        public ObservableCollection<Bond> Bonds { get; private set; }

        public EditUserBondViewModel(UserBond userBond, APIClient apiClient)
        {
            UserBond = userBond;
            _apiClient = apiClient;
            Users = new ObservableCollection<User>();
            Bonds = new ObservableCollection<Bond>();

            CancelCommand = new RelayCommand<object>(_ => CloseDialog?.Invoke(false));

            _ = LoadDataAsync();
        }

        public ICommand CancelCommand { get; }

        public int IdUser
        {
            get => UserBond.IdUser;
            set
            {
                UserBond.IdUser = value;
                OnPropertyChanged(nameof(IdUser));
            }
        }

        public int IdBond
        {
            get => UserBond.IdBond;
            set
            {
                UserBond.IdBond = value;
                OnPropertyChanged(nameof(IdBond));
            }
        }

        public int RemainingClasses
        {
            get => UserBond.RemainingClasses;
            set
            {
                UserBond.RemainingClasses = value;
                OnPropertyChanged(nameof(RemainingClasses));
            }
        }

        public Func<bool?, Task> CloseDialog { get; set; }

        private async Task LoadDataAsync()
        {
            var users = await _apiClient.GetUsersAsync();
            Users.Clear();
            foreach (var user in users)
                Users.Add(user);

            var bonds = await _apiClient.GetBondsAsync();
            Bonds.Clear();
            foreach (var bond in bonds)
                Bonds.Add(bond);
        }

        public async Task SaveAsync()
        {
            try
            {
                if (UserBond.Id == 0)
                {
                    var result = await _apiClient.CreateUserBondAsync(UserBond.IdUser, UserBond.IdBond, UserBond.RemainingClasses);
                    if (result != null)
                    {
                        UserBond.Id = result.Id;
                        await CloseDialog?.Invoke(true);
                    }
                }
                else
                {
                    var success = await _apiClient.UpdateUserBondAsync(UserBond);
                    if (success)
                    {
                        await CloseDialog?.Invoke(true);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el bono del usuario: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
