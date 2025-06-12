using System.Windows.Input;
using System.Windows;
using WaveClubAppEscritorio2.Models;
using WaveClubAppEscritorio2.Services;
using System.Collections.ObjectModel;

namespace WaveClubAppEscritorio2.ViewModels.Bookings
{
    public class EditBookingViewModel : ViewModelBase
    {
        private readonly APIClient _apiClient;
        public Booking Booking { get; private set; }

        public ObservableCollection<Activity> Activities { get; private set; }
        public ObservableCollection<User> Users { get; private set; }

        public EditBookingViewModel(Booking booking, APIClient apiClient)
        {
            Booking = booking;
            _apiClient = apiClient;
            Activities = new ObservableCollection<Activity>();
            Users = new ObservableCollection<User>();

            CancelCommand = new RelayCommand<object>(_ => CloseDialog?.Invoke(false));

            _ = LoadDataAsync();
        }

        public ICommand CancelCommand { get; }

        public int IdActivity
        {
            get => Booking.IdActivity;
            set
            {
                Booking.IdActivity = value;
                OnPropertyChanged(nameof(IdActivity));
            }
        }

        public int IdUser
        {
            get => Booking.IdUser;
            set
            {
                Booking.IdUser = value;
                OnPropertyChanged(nameof(IdUser));
            }
        }

        public Func<bool?, Task> CloseDialog { get; set; }

        private async Task LoadDataAsync()
        {
            var activities = await _apiClient.GetActivitiesAsync();
            Activities.Clear();
            foreach (var act in activities)
                Activities.Add(act);

            var users = await _apiClient.GetUsersAsync();
            Users.Clear();
            foreach (var usr in users)
                Users.Add(usr);
        }

        public async Task SaveAsync()
        {
            try
            {
                if (Booking.Id == 0)
                {
                    var result = await _apiClient.CreateBookingAsync(Booking);
                    if (result != null)
                    {
                        Booking.Id = result.Id;
                        await CloseDialog?.Invoke(true);
                    }
                }
                else
                {
                    var success = await _apiClient.UpdateBookingAsync(Booking);
                    if (success)
                    {
                        await CloseDialog?.Invoke(true);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la reserva: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}