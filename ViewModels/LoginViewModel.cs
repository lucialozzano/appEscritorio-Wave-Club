using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using System.Net.Http;
using WaveClubAppEscritorio2.Services;

namespace WaveClubAppEscritorio2.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _username;
        private string _password;
        private bool _isLoading;
        private string _errorMessage;
        private readonly HttpClient _httpClient;
        private readonly APIClient _apiClient;

        public event PropertyChangedEventHandler PropertyChanged;

        public LoginViewModel()
        {
            _apiClient = new APIClient();

            LoginCommand = new RelayCommand<object>(async _ => await Login());
        }

        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public ICommand LoginCommand { get; }

        private async Task Login()
        {
            IsLoading = true;
            ErrorMessage = "";

            var loginResponse = await _apiClient.LoginAsync(Username, Password);

            IsLoading = false;

            if (loginResponse != null)
            {
                if (loginResponse.Rol.ToLower() == "admin")
                {
                    var mainWindow = new MainWindow();
                    mainWindow.Show();

                    Application.Current.MainWindow.Close();
                }
                else
                {
                    ErrorMessage = "No tienes permisos para acceder. Solo administradores.";
                }
            }
            else
            {
                ErrorMessage = "Usuario o contraseña incorrectos";
            }
        }


        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

