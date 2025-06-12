using System.Windows;
using WaveClubAppEscritorio2.Models;
using WaveClubAppEscritorio2.Services;
using WaveClubAppEscritorio2.ViewModels.Bonds;

namespace WaveClubAppEscritorio2.Views
{
    /// <summary>
    /// Lógica de interacción para EditBondWindow.xaml
    /// </summary>
    public partial class EditBondWindow : Window
    {
        private readonly EditBondViewModel _viewModel;
        private readonly BondsViewModel _bondsViewModel;

        public EditBondWindow(Bond bond, APIClient apiClient, bool isNewBond, BondsViewModel bondsViewModel)
        {
            InitializeComponent();

            _bondsViewModel = bondsViewModel;
            _viewModel = new EditBondViewModel(bond, apiClient);
            DataContext = _viewModel;

            if (isNewBond)
            {
                tittle.Text = "Crear Bono";
                btnSave.Content = "Crear";
            }
            else
            {
                tittle.Text = "Editar Bono";
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
                await _bondsViewModel.LoadBondsAsync();
            }
        }
    }
}
