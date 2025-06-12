using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WaveClubAppEscritorio2.ViewModels;

namespace WaveClubAppEscritorio2.Views
{
    public partial class LoginWindows : Window
    {
        public LoginWindows()
        {
            InitializeComponent();
            Application.Current.MainWindow = this;
            DataContext = new LoginViewModel();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            var viewModel = DataContext as ViewModels.LoginViewModel;
            if (viewModel != null)
            {
                viewModel.Password = passwordBox.Password;
            }
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }

}



