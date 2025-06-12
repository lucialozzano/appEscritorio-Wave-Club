using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WaveClubAppEscritorio2
{
    public class RelayCommand<T> : ICommand
    {
        private readonly Func<T, Task> _execute;
        private readonly Func<T, bool> _canExecute;

        public event EventHandler CanExecuteChanged;

        // Constructor que recibe la función a ejecutar y opcionalmente la función CanExecute
        public RelayCommand(Func<T, Task> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        // Determina si el comando puede ejecutarse
        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke((T)parameter) ?? true;
        }

        // Ejecuta la función cuando se invoca el comando
        public async void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                await _execute((T)parameter);
            }
        }

        // Notifica cuando el estado de CanExecute cambia
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}