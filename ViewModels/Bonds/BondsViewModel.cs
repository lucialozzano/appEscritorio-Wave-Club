using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using WaveClubAppEscritorio2.Models;
using WaveClubAppEscritorio2.Services;
using WaveClubAppEscritorio2.Views;

namespace WaveClubAppEscritorio2.ViewModels.Bonds
{
    public class BondsViewModel : ViewModelBase
    {
        private readonly APIClient _apiClient;

        public BondsViewModel(APIClient apiClient)
        {
            _apiClient = apiClient;
            Bonds = new ObservableCollection<Bond>();
            LoadBondsCommand = new RelayCommand<object>(async _ => await LoadBondsAsync());
            DeleteBondCommand = new RelayCommand<int>(async id => await DeleteBondAsync(id));
            EditBondCommand = new RelayCommand<int>(async id => await EditBondAsync(id));
            NavigateToEditBondCommand = new RelayCommand<object>(async _ => await NavigateToEditBond());

            _ = LoadBondsAsync();
        }

        public ObservableCollection<Bond> Bonds { get; }

        public ICommand LoadBondsCommand { get; }
        public ICommand DeleteBondCommand { get; }
        public ICommand EditBondCommand { get; }
        public ICommand NavigateToEditBondCommand { get; }

        public async Task LoadBondsAsync()
        {
            var allBonds = await _apiClient.GetBondsAsync();

            Bonds.Clear();
            foreach (var bond in allBonds.OrderBy(b => b.NameActivity))
            {
                Bonds.Add(bond);
            }
        }

        private async Task DeleteBondAsync(int id)
        {
            var success = await _apiClient.DeleteBondAsync(id);
            if (success)
            {
                var toRemove = Bonds.FirstOrDefault(b => b.Id == id);
                if (toRemove != null) Bonds.Remove(toRemove);
            }
        }

        private async Task EditBondAsync(int id)
        {
            var bond = Bonds.FirstOrDefault(b => b.Id == id);
            if (bond != null)
            {
                var window = new EditBondWindow(bond, _apiClient, false, this);
                if (window.ShowDialog() == true)
                    await LoadBondsAsync();
            }
        }

        private async Task NavigateToEditBond()
        {
            var newBond = new Bond();
            var window = new EditBondWindow(newBond, _apiClient, true, this);
            if (window.ShowDialog() == true)
                await LoadBondsAsync();
        }

        public async Task AddBondAsync(Bond bond)
        {
            var created = await _apiClient.CreateBondAsync(bond);
            if (created != null)
            {
                bond.Id = created.Id;
                Bonds.Add(created);
            }
            else
            {
                MessageBox.Show("Error al guardar el bono.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
