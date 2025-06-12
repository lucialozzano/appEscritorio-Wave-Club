using System.Windows;
using System.Windows.Controls;
using WaveClubAppEscritorio2.Services;
using WaveClubAppEscritorio2.Models;
using WaveClubAppEscritorio2.ViewModels.Activities;

namespace WaveClubAppEscritorio2.Views
{
    /// <summary>
    /// Lógica de interacción para ActivityWindows.xaml
    /// </summary>
    public partial class ActivityWindow : Window
    {
        public ActivityWindow(APIClient apiClient)
        {
            InitializeComponent();
            DataContext = new ActivitiesViewModel(apiClient);
        }

        private async void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.Row.Item is Activity newActivity && newActivity.Id == 0)
            {
                var viewModel = DataContext as ActivitiesViewModel;
                if (viewModel != null)
                {
                    await viewModel.AddActivityAsync(newActivity);
                }
            }
        }

        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is int id)
            {
                var resultado = MessageBox.Show(
                    "¿Estás seguro de que deseas eliminar esta actividad?",
                    "Confirmar eliminación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (resultado == MessageBoxResult.Yes)
                {
                    var vm = DataContext as ActivitiesViewModel;
                    if (vm?.DeleteActivityCommand.CanExecute(id) == true)
                    {
                        vm.DeleteActivityCommand.Execute(id);
                    }
                }
            }
        }
    }
}
