using System.Windows;
using WaveClubAppEscritorio2.Models;
using WaveClubAppEscritorio2.Services;
using WaveClubAppEscritorio2.ViewModels.Partners;

namespace WaveClubAppEscritorio2.Views.Partners
{
    /// <summary>
    /// Lógica de interacción para EditPartnerWindow.xaml
    /// </summary>
    public partial class EditPartnerWindow : Window
    {
        private readonly EditPartnerViewModel _viewModel;
        private readonly PartnersViewModel _partnersViewModel;

        public EditPartnerWindow(Partner partner, APIClient apiClient, bool isNew, PartnersViewModel partnersViewModel)
        {
            InitializeComponent();

            _partnersViewModel = partnersViewModel;
            _viewModel = new EditPartnerViewModel(partner, apiClient);
            DataContext = _viewModel;

            if (isNew)
            {
                tittle.Text = "Crear Socio";
                btnSave.Content = "Crear";
            }
            else
            {
                tittle.Text = "Editar Socio";
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
                await _partnersViewModel.LoadPartnersAsync();
            }
        }
    }
}
