using System.Windows;
using WaveClubAppEscritorio2.Services;
using WaveClubAppEscritorio2.Models;
using WaveClubAppEscritorio2.ViewModels.UserBonds;

namespace WaveClubAppEscritorio2.Views.UserBonds
{
    /// <summary>
    /// Lógica de interacción para EditUserBondWindow.xaml
    /// </summary>
    public partial class EditUserBondWindow : Window
    {
        private readonly EditUserBondViewModel _viewModel;
        private readonly UserBondsViewModel _userBondsViewModel;

        public EditUserBondWindow(UserBond userBond, APIClient apiClient, bool isNew, UserBondsViewModel userBondsViewModel)
        {
            InitializeComponent();

            _userBondsViewModel = userBondsViewModel;
            _viewModel = new EditUserBondViewModel(userBond, apiClient);
            DataContext = _viewModel;

            if (isNew)
            {
                tittle.Text = "Crear Bono de Usuario";
                btnSave.Content = "Crear";
            }
            else
            {
                tittle.Text = "Editar Bono de Usuario";
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
                await _userBondsViewModel.LoadUserBondsAsync();
            }
        }
    }
}
