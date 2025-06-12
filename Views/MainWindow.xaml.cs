using System.Windows;
using WaveClubAppEscritorio2.ViewModels;

namespace WaveClubAppEscritorio2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private void UsersButton_Click(object sender, RoutedEventArgs e)
        {
         
        }

        private void BondsButton_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void UserBondsButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ActivitiesButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void EmployeesButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void PartnersButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BookingsButton_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}