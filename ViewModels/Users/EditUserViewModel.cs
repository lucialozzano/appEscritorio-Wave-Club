using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using WaveClubAppEscritorio2.Models;
using WaveClubAppEscritorio2.Services;


namespace WaveClubAppEscritorio2.ViewModels.Users
{
    public class EditUserViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private readonly User _originalUser;
        private readonly APIClient _apiClient;

        public EditUserViewModel(User user, APIClient apiClient)
        {
            _apiClient = apiClient;

            _originalUser = user ?? new User();
            UserName = _originalUser.UserName;
            Name = _originalUser.Name;
            Surname = _originalUser.Surname;
            PhoneNumber = _originalUser.PhoneNumber ?? string.Empty;
            Password = _originalUser.Password;
            Dna = _originalUser.Dna;
            Rol = _originalUser.Rol;
            EmailAdress = _originalUser.EmailAdress;

            SaveCommand = new RelayCommand<object>(async _ => await SaveAsync(), _ => CanSave());
            CancelCommand = new RelayCommand<object>(async _ => await Cancel());
        }

        public bool IsEditMode => _originalUser.Id != 0;


        private string _userName;
        public string UserName
        {
            get => _userName;
            set
            {
                if (_userName != value)
                {
                    _userName = value;
                    OnPropertyChanged(nameof(UserName));
                }
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                    OnPropertyChanged(nameof(CanSave));
                }
            }
        }

        private string _surname;
        public string Surname
        {
            get => _surname;
            set
            {
                if (_surname != value)
                {
                    _surname = value;
                    OnPropertyChanged(nameof(Surname));
                    OnPropertyChanged(nameof(CanSave));
                }
            }
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (_phoneNumber != value)
                {
                    _phoneNumber = value;
                    OnPropertyChanged(nameof(PhoneNumber));
                    OnPropertyChanged(nameof(CanSave));
                }
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        private string _dna;
        public string Dna
        {
            get => _dna;
            set
            {
                if (_dna != value)
                {
                    _dna = value;
                    OnPropertyChanged(nameof(Dna));
                    OnPropertyChanged(nameof(CanSave));
                }
            }
        }

        private string _rol;
        public string Rol
        {
            get => _rol;
            set
            {
                if (_rol != value)
                {
                    _rol = value;
                    OnPropertyChanged(nameof(Rol));
                    OnPropertyChanged(nameof(CanSave));
                }
            }
        }

        private string _emailAdress;
        public string EmailAdress
        {
            get => _emailAdress;
            set
            {
                if (_emailAdress != value)
                {
                    _emailAdress = value;
                    OnPropertyChanged(nameof(EmailAdress));
                    OnPropertyChanged(nameof(CanSave));
                }
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public Action<bool?> CloseDialog { get; set; }

        public async Task SaveAsync()
        {
            if (!CanSave())
            {
                MessageBox.Show("Por favor, completa todos los campos correctamente antes de guardar.");
                return;
            }

            if (_originalUser.Id == 0)
            {
                var created = await CreateUserAsync();
                if (!created) return; 
            }
            else
            {
                await UpdateUserAsync(); 
            }

            CloseDialog?.Invoke(true);

            var viewModel = Application.Current.MainWindow.DataContext as UsersViewModel;
            if (viewModel != null)
            {
                await viewModel.LoadUsersAsync();
            }

        }

        private async Task<bool> CreateUserAsync()
        {
            var newUser = new User
            {
                UserName = UserName,
                Name = Name,
                Surname = Surname,
                PhoneNumber = PhoneNumber,
                Dna = Dna,
                Password = Dna,
                Rol = Rol,
                EmailAdress = EmailAdress
            };

            var wasCreated = await _apiClient.CreateUserAsync(newUser);
            if (wasCreated)
            {
                MessageBox.Show("Usuario creado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }
            else
            {
                MessageBox.Show("Error al crear el usuario. Intenta nuevamente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private async Task UpdateUserAsync()
        {
            _originalUser.Name = Name;
            _originalUser.Surname = Surname;
            _originalUser.PhoneNumber = PhoneNumber;
            _originalUser.Dna = Dna;
            _originalUser.Rol = Rol;
            _originalUser.EmailAdress = EmailAdress;

            var success = await _apiClient.UpdateUserAsync(_originalUser);
            if (success)
            {
                MessageBox.Show("Usuario actualizado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Error al actualizar el usuario. Intenta nuevamente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private Task Cancel()
        {
            CloseDialog?.Invoke(false);
            return Task.CompletedTask;
        }

        private bool CanSave()
        {
            bool isValid = !string.IsNullOrWhiteSpace(UserName) &&
                           !string.IsNullOrWhiteSpace(Name) &&
                           !string.IsNullOrWhiteSpace(Surname) &&
                           !string.IsNullOrEmpty(PhoneNumber) &&
                           !string.IsNullOrWhiteSpace(Dna) &&
                           !string.IsNullOrWhiteSpace(Rol) &&
                           !string.IsNullOrWhiteSpace(EmailAdress) &&
                           EmailAdress.Contains("@");

            return isValid;
        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
