using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using WaveClubAppEscritorio2.Models;
using WaveClubAppEscritorio2.Services;
using WaveClubAppEscritorio2.Views.Partners;

namespace WaveClubAppEscritorio2.ViewModels.Partners
{
    public class PartnersViewModel : ViewModelBase
    {
        private readonly APIClient _apiClient;

        public PartnersViewModel(APIClient apiClient)
        {
            _apiClient = apiClient;
            Partners = new ObservableCollection<Partner>();
            LoadPartnersCommand = new RelayCommand<object>(async _ => await LoadPartnersAsync());
            DeletePartnerCommand = new RelayCommand<int>(async id => await DeletePartnerAsync(id));
            EditPartnerCommand = new RelayCommand<int>(async id => await EditPartnerAsync(id));
            NavigateToEditPartnerCommand = new RelayCommand<object>(async _ => await NavigateToEditPartner());

            _ = LoadPartnersAsync();
        }

        public ObservableCollection<Partner> Partners { get; }

        public ICommand LoadPartnersCommand { get; }
        public ICommand DeletePartnerCommand { get; }
        public ICommand EditPartnerCommand { get; }
        public ICommand NavigateToEditPartnerCommand { get; }

        public async Task LoadPartnersAsync()
        {
            var allPartners = await _apiClient.GetPartnersAsync();
            Partners.Clear();
            foreach (var p in allPartners)
            {
                Partners.Add(p);
            }
        }

        private async Task DeletePartnerAsync(int id)
        {
            var success = await _apiClient.DeletePartnerAsync(id);
            if (success)
            {
                var toRemove = Partners.FirstOrDefault(p => p.Id == id);
                if (toRemove != null) Partners.Remove(toRemove);
            }
        }

        private async Task EditPartnerAsync(int id)
        {
            var partner = Partners.FirstOrDefault(p => p.Id == id);
            if (partner != null)
            {
                var window = new EditPartnerWindow(partner, _apiClient, false, this);
                if (window.ShowDialog() == true)
                    await LoadPartnersAsync();
            }
        }

        private async Task NavigateToEditPartner()
        {
            var newPartner = new Partner();
            var window = new EditPartnerWindow(newPartner, _apiClient, true, this);
            if (window.ShowDialog() == true)
                await LoadPartnersAsync();
        }

        public async Task AddPartnerAsync(Partner partner)
        {
            var created = await _apiClient.CreatePartnerAsync(partner);
            if (created != null)
            {
                partner.Id = created.Id;
                Partners.Add(created);
            }
            else
            {
                MessageBox.Show("Error al guardar el partner.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
