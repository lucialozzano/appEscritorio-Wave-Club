using System.Windows;
using WaveClubAppEscritorio2.Services;
using WaveClubAppEscritorio2.Models;
using WaveClubAppEscritorio2.ViewModels.Activities;

namespace WaveClubAppEscritorio2.Views
{
    /// <summary>
    /// Lógica de interacción para EditActivityWindow.xaml
    /// </summary>
    public partial class EditActivityWindow : Window
    {
        private readonly EditActivityViewModel _viewModel;
        private readonly ActivitiesViewModel _activitiesViewModel;

        public EditActivityWindow(Activity activity, APIClient apiClient, bool isNewActivity, ActivitiesViewModel activitiesViewModel)
        {
            InitializeComponent();

            _activitiesViewModel = activitiesViewModel;
            _viewModel = new EditActivityViewModel(activity, apiClient);
            DataContext = _viewModel;

            if (isNewActivity)
            {
                tittle.Text = "Crear Actividad";
                btnSave.Content = "Crear";
            }
            else
            {
                tittle.Text = "Editar Actividad";
                btnSave.Content = "Guardar";
            }

            _viewModel.CloseDialog = (result) =>
            {
                DialogResult = result;
            };
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.SaveAsync();

            if (DialogResult == true)
            {
                await _activitiesViewModel.LoadActivitiesAsync();
            }
        }
    }
}
