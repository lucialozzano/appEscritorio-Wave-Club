using System.Windows;
using System.Windows.Controls;
using WaveClubAppEscritorio2.Models;
using WaveClubAppEscritorio2.Services;
using WaveClubAppEscritorio2.ViewModels.Bonds;

namespace WaveClubAppEscritorio2.Views
{
    /// <summary>
    /// Lógica de interacción para BondWindows.xaml
    /// </summary>
    public partial class BondWindow : Window
    {
        public BondWindow(APIClient apiClient)
        {
            InitializeComponent();
            DataContext = new BondsViewModel(apiClient);
        }

        private async void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.Row.Item is Bond newBond && newBond.Id == 0)
            {
                var viewModel = DataContext as BondsViewModel;
                if (viewModel != null)
                {
                    await viewModel.AddBondAsync(newBond);
                }
            }
        }

        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is int id)
            {
                var resultado = MessageBox.Show(
                    "¿Estás seguro de que deseas eliminar este bono?",
                    "Confirmar eliminación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (resultado == MessageBoxResult.Yes)
                {
                    var vm = DataContext as BondsViewModel; 
                    if (vm?.DeleteBondCommand.CanExecute(id) == true)
                    {
                        vm.DeleteBondCommand.Execute(id);
                    }
                }
            }
        }
    }
}