using System.Windows;
using System.Windows.Controls;
using WaveClubAppEscritorio2.Services;
using WaveClubAppEscritorio2.ViewModels.Employees;

namespace WaveClubAppEscritorio2.Views
{
    public partial class EmployeeWindows : Window
    {
        public EmployeeWindows(APIClient apiClient)
        {
            InitializeComponent();
            DataContext = new EmployeesViewModel(apiClient);
        }

        private async void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.Row.Item is WaveClubAppEscritorio2.Models.Employee emp && emp.Id == 0)
            {
                var vm = DataContext as EmployeesViewModel;
                if (vm != null)
                {
                    await vm.AddEmployeeAsync(emp);
                }
            }
        }

        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is int id)
            {
                var resultado = MessageBox.Show(
                    "¿Estás seguro de eliminar este empleado?",
                    "Confirmar eliminación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (resultado == MessageBoxResult.Yes)
                {
                    var vm = DataContext as EmployeesViewModel;
                    if (vm?.DeleteEmployeeCommand.CanExecute(id) == true)
                    {
                        vm.DeleteEmployeeCommand.Execute(id);
                    }
                }
            }
        }
    }
}