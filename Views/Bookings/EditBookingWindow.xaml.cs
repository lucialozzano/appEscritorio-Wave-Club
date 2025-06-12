using System.Windows;
using WaveClubAppEscritorio2.Services;
using WaveClubAppEscritorio2.Models;
using WaveClubAppEscritorio2.ViewModels.Bookings;

namespace WaveClubAppEscritorio2.Views
{
    /// <summary>
    /// Lógica de interacción para EditBookingWindow.xaml
    /// </summary>
    public partial class EditBookingWindow : Window
    {
        private readonly EditBookingViewModel _viewModel;
        private readonly BookingsViewModel _bookingsViewModel;

        public EditBookingWindow(Booking booking, APIClient apiClient, bool isNewBooking, BookingsViewModel bookingsViewModel)
        {
            InitializeComponent();

            _bookingsViewModel = bookingsViewModel;
            _viewModel = new EditBookingViewModel(booking, apiClient);
            DataContext = _viewModel;

            if (isNewBooking)
            {
                tittle.Text = "Crear Reserva";
                btnSave.Content = "Crear";
            }
            else
            {
                tittle.Text = "Editar Reserva";
                btnSave.Content = "Guardar";
            }

            _viewModel.CloseDialog = (result) =>
            {
                DialogResult = result;
                return Task.CompletedTask;
            };
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.SaveAsync();

            if (DialogResult == true)
            {
                await _bookingsViewModel.LoadBookingsAsync();
            }
        }
    }
}
