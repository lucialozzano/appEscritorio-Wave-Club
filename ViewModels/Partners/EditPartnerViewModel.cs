using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using WaveClubAppEscritorio2.Models;
using WaveClubAppEscritorio2.Services;

namespace WaveClubAppEscritorio2.ViewModels.Partners
{
    public class EditPartnerViewModel : ViewModelBase
    {
        private readonly Partner _originalPartner;
        private readonly APIClient _apiClient;

        public EditPartnerViewModel(Partner partner, APIClient apiClient)
        {
            _apiClient = apiClient;
            _originalPartner = partner ?? new Partner();

            SaveCommand = new RelayCommand<object>(async _ => await SaveAsync(), _ => CanSave());
            CancelCommand = new RelayCommand<object>(_ =>
            {
                CloseDialog?.Invoke(false);
                return Task.CompletedTask;
            });
        }

        public bool IsEditMode => _originalPartner.Id != 0;

        public string UserName
        {
            get => _originalPartner.UserName;
            set
            {
                if (_originalPartner.UserName != value)
                {
                    _originalPartner.UserName = value;
                    OnPropertyChanged(nameof(UserName));
                }
            }
        }

        public string Name
        {
            get => _originalPartner.Name;
            set
            {
                if (_originalPartner.Name != value)
                {
                    _originalPartner.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string Surname
        {
            get => _originalPartner.Surname;
            set
            {
                if (_originalPartner.Surname != value)
                {
                    _originalPartner.Surname = value;
                    OnPropertyChanged(nameof(Surname));
                }
            }
        }

        public string PhoneNumber
        {
            get => _originalPartner.PhoneNumber;
            set
            {
                if (_originalPartner.PhoneNumber != value)
                {
                    _originalPartner.PhoneNumber = value;
                    OnPropertyChanged(nameof(PhoneNumber));
                }
            }
        }

        public string Password
        {
            get => _originalPartner.Password;
            set
            {
                if (_originalPartner.Password != value)
                {
                    _originalPartner.Password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        public string Dna
        {
            get => _originalPartner.Dna;
            set
            {
                if (_originalPartner.Dna != value)
                {
                    _originalPartner.Dna = value;
                    OnPropertyChanged(nameof(Dna));
                }
            }
        }

        public string Rol
        {
            get => _originalPartner.Rol;
            set
            {
                if (_originalPartner.Rol != value)
                {
                    _originalPartner.Rol = value;
                    OnPropertyChanged(nameof(Rol));
                }
            }
        }

        public string EmailAdress
        {
            get => _originalPartner.EmailAdress;
            set
            {
                if (_originalPartner.EmailAdress != value)
                {
                    _originalPartner.EmailAdress = value;
                    OnPropertyChanged(nameof(EmailAdress));
                }
            }
        }

        public bool IsDisabled
        {
            get => _originalPartner.IsDisabled;
            set
            {
                if (_originalPartner.IsDisabled != value)
                {
                    _originalPartner.IsDisabled = value;
                    OnPropertyChanged(nameof(IsDisabled));
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
                   !string.IsNullOrWhiteSpace(EmailAdress);
        }

        public async Task SaveAsync()
        {
            if (!CanSave())
            {
                MessageBox.Show("Completa todos los campos antes de guardar.");
                return;
            }

            _originalPartner.Password = _originalPartner.Dna;
            _originalPartner.Rol = "partner";

            if (_originalPartner.Id == 0)
            {
                var created = await _apiClient.CreatePartnerAsync(_originalPartner);
                if (created != null)
                {
                    _originalPartner.Id = created.Id;
                    MessageBox.Show("Partner creado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Error al crear el partner.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                var success = await _apiClient.UpdatePartnerAsync(_originalPartner);
                if (success)
                {
                    MessageBox.Show("Partner actualizado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Error al actualizar el partner.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            CloseDialog?.Invoke(true);

            if (Application.Current.MainWindow.DataContext is PartnersViewModel viewModel)
                await viewModel.LoadPartnersAsync();
        }
    }
}
