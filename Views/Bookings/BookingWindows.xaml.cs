using System.Windows;
using System.Windows.Controls;
using WaveClubAppEscritorio2.Models;
using WaveClubAppEscritorio2.Services;
using WaveClubAppEscritorio2.ViewModels.Bookings;


namespace WaveClubAppEscritorio2.Views.Bookings
{
    /// <summary>
    /// Lógica de interacción para BookingWindows.xaml
    /// </summary>
    public partial class BookingWindow : Window
    {
        public BookingWindow(APIClient apiClient)
        {
            InitializeComponent();
            DataContext = new BookingsViewModel(apiClient);
        }

        private async void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.Row.Item is Booking newBooking && newBooking.Id == 0) 
            {
                var viewModel = DataContext as BookingsViewModel;
                if (viewModel != null)
                {
                    await viewModel.AddBookingAsync(newBooking);
                }
            }
        }

        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is int id)
            {
                var resultado = MessageBox.Show(
                    "¿Estás seguro de que deseas eliminar esta reserva?",
                    "Confirmar eliminación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (resultado == MessageBoxResult.Yes)
                {
                    var vm = DataContext as BookingsViewModel;
                    if (vm?.DeleteBookingCommand.CanExecute(id) == true)
                    {
                        vm.DeleteBookingCommand.Execute(id);
                    }
                }
            }
        }
    }
}
