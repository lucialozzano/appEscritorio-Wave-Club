using System.Collections.ObjectModel;
using System.Windows.Input;
using WaveClubAppEscritorio2.Models;
using WaveClubAppEscritorio2.Services;
using WaveClubAppEscritorio2.Views;

namespace WaveClubAppEscritorio2.ViewModels.Bookings
{
    public class BookingsViewModel : ViewModelBase
    {
        private readonly APIClient _apiClient;

        public BookingsViewModel(APIClient apiClient)
        {
            _apiClient = apiClient;
            Bookings = new ObservableCollection<Booking>();

            LoadBookingsCommand = new RelayCommand<object>(async _ => await LoadBookingsAsync());
            DeleteBookingCommand = new RelayCommand<int>(async id => await DeleteBookingAsync(id));
            EditBookingCommand = new RelayCommand<int>(async id => await EditBookingAsync(id));
            NavigateToEditBookingCommand = new RelayCommand<object>(async _ => await NavigateToEditBooking());

            _ = LoadBookingsAsync();
        }

        public ObservableCollection<Booking> Bookings { get; }

        public ICommand LoadBookingsCommand { get; }
        public ICommand DeleteBookingCommand { get; }
        public ICommand EditBookingCommand { get; }
        public ICommand NavigateToEditBookingCommand { get; }

        public async Task LoadBookingsAsync()
        {
            var allBookings = await _apiClient.GetBookingsAsync();

            Bookings.Clear();
            foreach (var booking in allBookings.OrderBy(b => b.Activity?.Name))
            {
                Bookings.Add(booking);
            }
        }

        private async Task DeleteBookingAsync(int id)
        {
            var success = await _apiClient.DeleteBookingAsync(id);
            if (success)
            {
                var toRemove = Bookings.FirstOrDefault(b => b.Id == id);
                if (toRemove != null) Bookings.Remove(toRemove);
            }
        }

        private async Task EditBookingAsync(int id)
        {
            var booking = Bookings.FirstOrDefault(b => b.Id == id);
            if (booking != null)
            {
                var window = new EditBookingWindow(booking, _apiClient, false, this);
                if (window.ShowDialog() == true)
                    await LoadBookingsAsync();
            }
        }

        private async Task NavigateToEditBooking()
        {
            var newBooking = new Booking();
            var window = new EditBookingWindow(newBooking, _apiClient, true, this);
            if (window.ShowDialog() == true)
                await LoadBookingsAsync();
        }

        public async Task AddBookingAsync(Booking newBooking)
        {
            var createdBooking = await _apiClient.CreateBookingAsync(newBooking);
            if (createdBooking != null)
            {
                Bookings.Add(createdBooking);
            }
        }
    }
}