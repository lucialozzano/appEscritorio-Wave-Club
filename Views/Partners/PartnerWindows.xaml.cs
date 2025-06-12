using System.Windows;
using System.Windows.Controls;
using WaveClubAppEscritorio2.Models;
using WaveClubAppEscritorio2.Services;
using WaveClubAppEscritorio2.ViewModels.Partners;

namespace WaveClubAppEscritorio2.Views
{
    public partial class PartnerWindows : Window
    {
        public PartnerWindows(APIClient apiClient)
        {
            InitializeComponent();
            DataContext = new PartnersViewModel(apiClient);
        }

        private async void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.Row.Item is Partner partner && partner.Id == 0)
            {
                var vm = DataContext as PartnersViewModel;
                if (vm != null)
                {
                    await vm.AddPartnerAsync(partner);
                }
            }
        }

        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is int id)
            {
                var result = MessageBox.Show(
                    "¿Estás seguro de eliminar este partner?",
                    "Confirmar eliminación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    var vm = DataContext as PartnersViewModel;
                    if (vm?.DeletePartnerCommand.CanExecute(id) == true)
                    {
                        vm.DeletePartnerCommand.Execute(id);
                    }
                }
            }
        }
    }
}
