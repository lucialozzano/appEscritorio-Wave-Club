using System.Windows;
using WaveClubAppEscritorio2.Models;
using WaveClubAppEscritorio2.Services;
using WaveClubAppEscritorio2.ViewModels.Users;

namespace WaveClubAppEscritorio2.Views
{

    public partial class EditUserWindow : Window
    {
        private readonly EditUserViewModel _viewModel;
        private readonly UsersViewModel _usersViewModel;

        public EditUserWindow(User user, APIClient apiClient, bool isNewUser, UsersViewModel usersViewModel)
        {
            InitializeComponent();

            _usersViewModel = usersViewModel;
            _viewModel = new EditUserViewModel(user, apiClient);
            DataContext = _viewModel;

            if (isNewUser)
            {
                tittle.Text = "Crear Usuario";
                btnSave.Content = "Crear";
            }
            else
            {
                tittle.Text = "Editar Usuario";
                btnSave.Content = "Guardar";
            }

            _viewModel.CloseDialog = (result) =>
            {
                if (result == true)
                {
                    DialogResult = true;
                }
                else
                {
                    DialogResult = false;
                }
            };
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.SaveAsync();

            if (DialogResult == true)
            {
                await _usersViewModel.LoadUsersAsync();
            }
        }
    }

}
