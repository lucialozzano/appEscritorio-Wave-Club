using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using WaveClubAppEscritorio2.Models;
using WaveClubAppEscritorio2.Services;
using WaveClubAppEscritorio2.Views.Employees;


namespace WaveClubAppEscritorio2.ViewModels.Employees
{
    public class EmployeesViewModel : ViewModelBase
    {
        private readonly APIClient _apiClient;

        public EmployeesViewModel(APIClient apiClient)
        {
            _apiClient = apiClient;
            Employees = new ObservableCollection<Employee>();
            LoadEmployeesCommand = new RelayCommand<object>(async _ => await LoadEmployeesAsync());
            DeleteEmployeeCommand = new RelayCommand<int>(async id => await DeleteEmployeeAsync(id));
            EditEmployeeCommand = new RelayCommand<int>(async id => await EditEmployeeAsync(id));

            NavigateToEditEmployeeCommand = new RelayCommand<object>(async _ => await NavigateToEditEmployee());

            _ = LoadEmployeesAsync();
        }

        public ObservableCollection<Employee> Employees { get; }

        public ICommand LoadEmployeesCommand { get; }
        public ICommand DeleteEmployeeCommand { get; }
        public ICommand EditEmployeeCommand { get; }
        public ICommand NavigateToEditEmployeeCommand { get; }

        public async Task LoadEmployeesAsync()
        {
            var allEmployees = await _apiClient.GetEmployeesAsync();
            Employees.Clear();
            foreach (var e in allEmployees)
            {
                Employees.Add(e);
            }
        }

        private async Task DeleteEmployeeAsync(int id)
        {
            var success = await _apiClient.DeleteEmployeeAsync(id);
            if (success)
            {
                var toRemove = Employees.FirstOrDefault(e => e.Id == id);
                if (toRemove != null) Employees.Remove(toRemove);
            }
        }

        private async Task EditEmployeeAsync(int id)
        {
            var employee = Employees.FirstOrDefault(e => e.Id == id);
            if (employee != null)
            {
                var window = new EditEmployeeWindow(employee, _apiClient, false, this);
                if (window.ShowDialog() == true)
                    await LoadEmployeesAsync();
            }
        }

        private async Task NavigateToEditEmployee()
        {
            var newEmployee = new Employee();
            var window = new EditEmployeeWindow(newEmployee, _apiClient, true, this);
            if (window.ShowDialog() == true)
                await LoadEmployeesAsync();
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
            var created = await _apiClient.CreateEmployeeAsync(employee);
            if (created != null)
            {
                employee.Id = created.Id;
                Employees.Add(created);
            }
            else
            {
                MessageBox.Show("Error al guardar el empleado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
