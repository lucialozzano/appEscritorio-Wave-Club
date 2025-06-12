using System.Windows.Input;
using System.Windows;
using WaveClubAppEscritorio2.Models;
using WaveClubAppEscritorio2.Services;

namespace WaveClubAppEscritorio2.ViewModels.Bonds
{
    public class EditBondViewModel : ViewModelBase
    {
        private readonly APIClient _apiClient;
        public Bond Bond { get; private set; }

        public EditBondViewModel(Bond bond, APIClient apiClient)
        {
            Bond = bond;
            _apiClient = apiClient;
            CancelCommand = new RelayCommand<object>(_ => CloseDialog?.Invoke(false));
        }

        public ICommand CancelCommand { get; }

        public string NameActivity
        {
            get => Bond.NameActivity;
            set
            {
                Bond.NameActivity = value;
                OnPropertyChanged(nameof(NameActivity));
            }
        }

        public string Description
        {
            get => Bond.Description;
            set
            {
                Bond.Description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public int Level
        {
            get => Bond.Level;
            set
            {
                Bond.Level = value;
                OnPropertyChanged(nameof(Level));
            }
        }

        public double Price
        {
            get => Bond.Price;
            set
            {
                Bond.Price = value;
                OnPropertyChanged(nameof(Price));
            }
        }

        public Func<bool?, Task> CloseDialog { get; set; }

        public async Task SaveAsync()
        {
            try
            {
                if (Bond.Id == 0)
                {
                    var result = await _apiClient.CreateBondAsync(Bond);
                    if (result != null)
                    {
                        Bond.Id = result.Id;
                        await CloseDialog?.Invoke(true);
                    }
                }
                else
                {
                    var success = await _apiClient.UpdateBondAsync(Bond);
                    if (success)
                    {
                        await CloseDialog?.Invoke(true);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el bono: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
