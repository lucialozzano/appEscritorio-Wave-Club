using System.ComponentModel;
using System.Windows.Input;
using WaveClubAppEscritorio2.Models;
using System.Windows;
using WaveClubAppEscritorio2.Services;

namespace WaveClubAppEscritorio2.ViewModels.Activities
{
    public class EditActivityViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private readonly Activity _originalActivity;
        private readonly APIClient _apiClient;

        public EditActivityViewModel(Activity activity, APIClient apiClient)
        {
            _apiClient = apiClient;

            _originalActivity = activity ?? new Activity();
            Name = _originalActivity.Name;
            Date = _originalActivity.Date == DateTime.MinValue ? DateTime.Today : _originalActivity.Date;
            Start = _originalActivity.Start;
            End = _originalActivity.End;
            Capacity = _originalActivity.Capacity;
            RemainingClasses = _originalActivity.RemainingClasses;
            Level = _originalActivity.Level;
            DayWeek = _originalActivity.DayWeek;

            SaveCommand = new RelayCommand<object>(async _ => await SaveAsync(), _ => CanSave());
            CancelCommand = new RelayCommand<object>(async _ => await Cancel());
        }

        public bool IsEditMode => _originalActivity.Id != 0;

        private string _name;
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); OnPropertyChanged(nameof(CanSave)); }
        }

        private DateTime _date;
        public DateTime Date
        {
            get => _date;
            set { _date = value; OnPropertyChanged(nameof(Date)); OnPropertyChanged(nameof(CanSave)); }
        }

        private string _start;
        public string Start
        {
            get => _start;
            set { _start = value; OnPropertyChanged(nameof(Start)); OnPropertyChanged(nameof(CanSave)); }
        }

        private string _end;
        public string End
        {
            get => _end;
            set { _end = value; OnPropertyChanged(nameof(End)); OnPropertyChanged(nameof(CanSave)); }
        }

        private int _capacity;
        public int Capacity
        {
            get => _capacity;
            set { _capacity = value; OnPropertyChanged(nameof(Capacity)); OnPropertyChanged(nameof(CanSave)); }
        }

        private int _remainingClasses;
        public int RemainingClasses
        {
            get => _remainingClasses;
            set { _remainingClasses = value; OnPropertyChanged(nameof(RemainingClasses)); OnPropertyChanged(nameof(CanSave)); }
        }

        private int _level;
        public int Level
        {
            get => _level;
            set { _level = value; OnPropertyChanged(nameof(Level)); OnPropertyChanged(nameof(CanSave)); }
        }

        private string _dayWeek;
        public string DayWeek
        {
            get => _dayWeek;
            set { _dayWeek = value; OnPropertyChanged(nameof(DayWeek)); OnPropertyChanged(nameof(CanSave)); }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public Action<bool?> CloseDialog { get; set; }

        public async Task SaveAsync()
        {
            if (!CanSave())
            {
                MessageBox.Show("Completa todos los campos antes de guardar.");
                return;
            }

            if (_originalActivity.Id == 0)
                await CreateActivityAsync();
            else
                await UpdateActivityAsync();

            CloseDialog?.Invoke(true);

            if (Application.Current.MainWindow.DataContext is ActivitiesViewModel viewModel)
                await viewModel.LoadActivitiesAsync();
        }

        private async Task CreateActivityAsync()
        {
            var newActivity = new Activity
            {
                Name = Name,
                Date = Date,
                Start = Start,
                End = End,
                Capacity = Capacity,
                Level = Level,
                DayWeek = DayWeek,
                RemainingClasses = RemainingClasses,
                IsComplete = false
            };

            var created = await _apiClient.CreateActivityAsync(newActivity);
            if (created != null)
                MessageBox.Show("Actividad creada exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("Error al crear la actividad.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private async Task UpdateActivityAsync()
        {
            _originalActivity.Name = Name;
            _originalActivity.Date = Date;
            _originalActivity.Start = Start;
            _originalActivity.End = End;
            _originalActivity.Capacity = Capacity;
            _originalActivity.RemainingClasses = RemainingClasses;
            _originalActivity.Level = Level;
            _originalActivity.DayWeek = DayWeek;

            var success = await _apiClient.UpdateActivityAsync(_originalActivity);
            if (success)
                MessageBox.Show("Actividad actualizada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("Error al actualizar la actividad.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private Task Cancel()
        {
            CloseDialog?.Invoke(false);
            return Task.CompletedTask;
        }

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(Name) &&
                   !string.IsNullOrWhiteSpace(Start) &&
                   !string.IsNullOrWhiteSpace(End) &&
                   !string.IsNullOrWhiteSpace(DayWeek) &&
                   Capacity > 0 &&
                   Level >= 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}