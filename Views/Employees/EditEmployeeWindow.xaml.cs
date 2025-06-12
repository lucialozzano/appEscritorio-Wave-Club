using System.Windows;
using WaveClubAppEscritorio2.Models;
using WaveClubAppEscritorio2.Services;
using WaveClubAppEscritorio2.ViewModels.Employees;

namespace WaveClubAppEscritorio2.Views.Employees
{
    public partial class EditEmployeeWindow : Window
    {
        private readonly EditEmployeeViewModel _viewModel;
        private readonly EmployeesViewModel _employeesViewModel;

        public EditEmployeeWindow(Employee employee, APIClient apiClient, bool isNew, EmployeesViewModel employeesViewModel)
        {
            InitializeComponent();

            _employeesViewModel = employeesViewModel;
            _viewModel = new EditEmployeeViewModel(employee, apiClient);
            DataContext = _viewModel;

            if (isNew)
            {
                tittle.Text = "Crear Empleado";
                btnSave.Content = "Crear";
            }
            else
            {
                tittle.Text = "Editar Empleado";
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
                await _employeesViewModel.LoadEmployeesAsync();
            }
        }
    }
}
