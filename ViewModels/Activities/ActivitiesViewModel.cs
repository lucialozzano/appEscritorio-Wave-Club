using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using WaveClubAppEscritorio2.Services;
using WaveClubAppEscritorio2.Views;
using WaveClubAppEscritorio2.Models;


namespace WaveClubAppEscritorio2.ViewModels.Activities
{
    public class ActivitiesViewModel : ViewModelBase
    {
        private readonly APIClient _apiClient;


        public ActivitiesViewModel(APIClient apiClient)
        {
            NavigateToEditActivityCommand = new RelayCommand<object>(async _ => await NavigateToEditActivity());
            _apiClient = apiClient;
            LoadActivitiesCommand = new RelayCommand<object>(async _ => await LoadActivitiesAsync());
            DeleteActivityCommand = new RelayCommand<int>(async id => await DeleteActivityAsync(id));
            EditActivityCommand = new RelayCommand<int>(async id => await EditActivityAsync(id));
            Activities = new ObservableCollection<Activity>();

            _ = LoadActivitiesAsync();
        }

        public ObservableCollection<Activity> Activities { get; }

        public ICommand LoadActivitiesCommand { get; }
        public ICommand DeleteActivityCommand { get; }
        public ICommand EditActivityCommand { get; }
        public ICommand NavigateToEditActivityCommand { get; }


        public async Task LoadActivitiesAsync()
        {
            var allActivities = await _apiClient.GetActivitiesAsync();

            var desdeFecha = DateTime.Now.AddMonths(-1);

            var actividadesFiltradas = allActivities
                .Where(a => a.Date >= desdeFecha)
                .OrderBy(a => a.Date)
                .ToList();

            Activities.Clear();
            foreach (var act in actividadesFiltradas)
            {
                Activities.Add(act);
            }
        }

        private async Task DeleteActivityAsync(int id)
        {
            var success = await _apiClient.DeleteActivityAsync(id);
            if (success)
            {
                var toRemove = Activities.FirstOrDefault(a => a.Id == id);
                if (toRemove != null) Activities.Remove(toRemove);
            }
        }

        private async Task EditActivityAsync(int id)
        {
            var activity = Activities.FirstOrDefault(a => a.Id == id);
            if (activity != null)
            {
                var window = new EditActivityWindow(activity, _apiClient, false, this);
                if (window.ShowDialog() == true)
                    await LoadActivitiesAsync();
            }
        }

        private async Task NavigateToEditActivity()
        {
            var newActivity = new Activity();
            var window = new EditActivityWindow(newActivity, _apiClient, true, this);
            if (window.ShowDialog() == true)
                await LoadActivitiesAsync();
        }

        public async Task AddActivityAsync(Activity activity)
        {
            var created = await _apiClient.CreateActivityAsync(activity);
            if (created != null)
            {
                activity.Id = created.Id;
                Activities.Add(created);
            }
            else
            {
                MessageBox.Show("Error al guardar la actividad.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}