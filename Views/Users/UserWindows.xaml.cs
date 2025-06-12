using System.Windows;
using System.Windows.Controls;
using WaveClubAppEscritorio2.Models;
using WaveClubAppEscritorio2.Services;
using WaveClubAppEscritorio2.ViewModels.Users;

namespace WaveClubAppEscritorio2.Views
{
    /// <summary>
    /// Lógica de interacción para UserWindows.xaml
    /// </summary>
    public partial class UserWindows : Window
    {
        public UserWindows(APIClient apiClient)
        {
            InitializeComponent();
            DataContext = new UsersViewModel(apiClient);
        }
        private async void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.Row.Item is User newUser && newUser.Id == 0) 
            {
                var viewModel = DataContext as UsersViewModel;
                if (viewModel != null)
                {
                    await viewModel.AddUserAsync(newUser);
                }
            }
        }

        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is int id)
            {
                var resultado = MessageBox.Show(
                    "¿Estás seguro de que deseas eliminar este usuario?",
                    "Confirmar eliminación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (resultado == MessageBoxResult.Yes)
                {
                    var vm = DataContext as UsersViewModel;
                    if (vm?.DeleteUserCommand.CanExecute(id) == true)
                    {
                        vm.DeleteUserCommand.Execute(id);
                    }
                }
            }
        }
    }
}
