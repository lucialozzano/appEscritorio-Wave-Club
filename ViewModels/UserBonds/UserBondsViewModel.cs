
using System.Collections.ObjectModel;
using System.Windows.Input;
using WaveClubAppEscritorio2.Services;
using WaveClubAppEscritorio2.Models;
using WaveClubAppEscritorio2.Views.UserBonds;

namespace WaveClubAppEscritorio2.ViewModels.UserBonds
{
    public class UserBondsViewModel : ViewModelBase
    {
        private readonly APIClient _apiClient;

        public UserBondsViewModel(APIClient apiClient)
        {
            _apiClient = apiClient;
            UserBonds = new ObservableCollection<UserBond>();

            LoadUserBondsCommand = new RelayCommand<object>(async _ => await LoadUserBondsAsync());
            DeleteUserBondCommand = new RelayCommand<int>(async id => await DeleteUserBondAsync(id));
            EditUserBondCommand = new RelayCommand<int>(async id => await EditUserBondAsync(id));
            NavigateToEditUserBondCommand = new RelayCommand<object>(async _ => await NavigateToEditUserBond());


            _ = LoadUserBondsAsync();
        }

        public ObservableCollection<UserBond> UserBonds { get; }

        public ICommand LoadUserBondsCommand { get; }
        public ICommand DeleteUserBondCommand { get; }
        public ICommand EditUserBondCommand { get; }
        public ICommand NavigateToEditUserBondCommand { get; }

        private async Task NavigateToEditUserBond()
        {
            var newUserBond = new UserBond();
            var window = new EditUserBondWindow(newUserBond, _apiClient, true, this);
            if (window.ShowDialog() == true)
                await LoadUserBondsAsync();
        }

        public async Task LoadUserBondsAsync()
        {
            var bonds = await _apiClient.GetUserBondsAsync();

            UserBonds.Clear();
            foreach (var ub in bonds.OrderBy(ub => ub.IdBond))
            {
                UserBonds.Add(ub);
            }
        }


        private async Task DeleteUserBondAsync(int id)
        {
            var success = await _apiClient.DeleteUserBondAsync(id);
            if (success)
            {
                var toRemove = UserBonds.FirstOrDefault(ub => ub.Id == id);
                if (toRemove != null) UserBonds.Remove(toRemove);
            }
        }

        private async Task EditUserBondAsync(int id)
        {
            var userBond = UserBonds.FirstOrDefault(ub => ub.Id == id);
            if (userBond != null)
            {
                var window = new EditUserBondWindow(userBond, _apiClient, false, this);
                if (window.ShowDialog() == true)
                    await LoadUserBondsAsync();
            }
        }

        public async Task AddUserBondAsync(UserBond userBond)
        {
            var result = await _apiClient.CreateUserBondAsync(userBond.IdUser, userBond.IdBond, userBond.RemainingClasses);
            if (result != null)
            {
                UserBonds.Add(result);
            }
        }

        public async Task UpdateUserBondAsync(UserBond bond)
        {
            var success = await _apiClient.UpdateUserBondAsync(bond);
            if (success)
            {
                await LoadUserBondsAsync();
            }
        }
    }
}