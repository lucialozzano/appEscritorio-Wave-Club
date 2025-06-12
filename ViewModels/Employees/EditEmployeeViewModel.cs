using System.Windows.Input;
using System.Windows;
using WaveClubAppEscritorio2.Models;
using WaveClubAppEscritorio2.Services;

namespace WaveClubAppEscritorio2.ViewModels.Employees
{
    public class EditEmployeeViewModel : ViewModelBase
    {
        private readonly Employee _originalEmployee;
        private readonly APIClient _apiClient;

        public EditEmployeeViewModel(Employee employee, APIClient apiClient)
        {
            _apiClient = apiClient;
            _originalEmployee = employee ?? new Employee();

            SaveCommand = new RelayCommand<object>(async _ => await SaveAsync(), _ => CanSave());
            CancelCommand = new RelayCommand<object>(_ =>
            {
                CloseDialog?.Invoke(false);
                return Task.CompletedTask;
            });
        }

        public bool IsEditMode => _originalEmployee.Id != 0;

        public string UserName
        {
            get => _originalEmployee.UserName;
            set
            {
                if (_originalEmployee.UserName != value)
                {
                    _originalEmployee.UserName = value;
                    OnPropertyChanged(nameof(UserName));
                }
            }
        }

        public string Name
        {
            get => _originalEmployee.Name;
            set
            {
                if (_originalEmployee.Name != value)
                {
                    _originalEmployee.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string Surname
        {
            get => _originalEmployee.Surname;
            set
            {
                if (_originalEmployee.Surname != value)
                {
                    _originalEmployee.Surname = value;
                    OnPropertyChanged(nameof(Surname));
                }
            }
        }

        public string PhoneNumber
        {
            get => _originalEmployee.PhoneNumber;
            set
            {
                if (_originalEmployee.PhoneNumber != value)
                {
                    _originalEmployee.PhoneNumber = value;
                    OnPropertyChanged(nameof(PhoneNumber));
                }
            }
        }

        public string Password
        {
            get => _originalEmployee.Password;
            set
            {
                if (_originalEmployee.Password != value)
                {
                    _originalEmployee.Password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        public string Dna
        {
            get => _originalEmployee.Dna;
            set
            {
                if (_originalEmployee.Dna != value)
                {
                    _originalEmployee.Dna = value;
                    OnPropertyChanged(nameof(Dna));
                }
            }
        }

        public string Rol
        {
            get => _originalEmployee.Rol;
            set
            {
                if (_originalEmployee.Rol != value)
                {
                    _originalEmployee.Rol = value;
                    OnPropertyChanged(nameof(Rol));
                }
            }
        }

        public string EmailAdress
        {
            get => _originalEmployee.EmailAdress;
            set
            {
                if (_originalEmployee.EmailAdress != value)
                {
                    _originalEmployee.EmailAdress = value;
                    OnPropertyChanged(nameof(EmailAdress));
                }
            }
        }

        public string Department
        {
            get => _originalEmployee.Department;
            set
            {
                if (_originalEmployee.Department != value)
                {
                    _originalEmployee.Department = value;
                    OnPropertyChanged(nameof(Department));
                }
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Action<bool?> CloseDialog { get; set; }

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(UserName) &&
                   !string.IsNullOrWhiteSpace(Name) &&
                   !string.IsNullOrWhiteSpace(Surname) &&
                   !string.IsNullOrWhiteSpace(PhoneNumber) &&
                   !string.IsNullOrWhiteSpace(Dna) &&
                   !string.IsNullOrWhiteSpace(EmailAdress) &&
                   !string.IsNullOrWhiteSpace(Department);
        }

        public async Task SaveAsync()
        {
            if (!CanSave())
            {
                MessageBox.Show("Completa todos los campos antes de guardar.");
                return;
            }

            _originalEmployee.Password = _originalEmployee.Dna;
            _originalEmployee.Rol = "employee";

            if (_originalEmployee.Id == 0)
            {
                var created = await _apiClient.CreateEmployeeAsync(_originalEmployee);
                if (created != null)
                {
                    _originalEmployee.Id = created.Id;
                    MessageBox.Show("Empleado creado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Error al crear el empleado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                var success = await _apiClient.UpdateEmployeeAsync(_originalEmployee);
                if (success)
                {
                    MessageBox.Show("Empleado actualizado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Error al actualizar el empleado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            CloseDialog?.Invoke(true);

            if (Application.Current.MainWindow.DataContext is EmployeesViewModel viewModel)
                await viewModel.LoadEmployeesAsync();
        }
    }
}
