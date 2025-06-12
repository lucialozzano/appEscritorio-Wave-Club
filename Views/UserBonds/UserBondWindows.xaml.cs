using System.Windows;
using System.Windows.Controls;
using WaveClubAppEscritorio2.Services;
using WaveClubAppEscritorio2.ViewModels.UserBonds;

namespace WaveClubAppEscritorio2.Views.UserBonds
{
    public partial class UserBondWindows : Window
    {
        public UserBondWindows(APIClient apiClient)
        {
            InitializeComponent();
            DataContext = new UserBondsViewModel(apiClient);
        }

        private async void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.Row.Item is Models.UserBond newUserBond && newUserBond.Id == 0)
            {
                var viewModel = DataContext as UserBondsViewModel;
                if (viewModel != null)
                {
                    await viewModel.AddUserBondAsync(newUserBond);
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
                    var vm = DataContext as UserBondsViewModel;
                    if (vm?.DeleteUserBondCommand.CanExecute(id) == true)
                    {
                        vm.DeleteUserBondCommand.Execute(id);
                    }
                }
            }
        }
    }
}
