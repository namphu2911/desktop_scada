using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork
{
    public class BaseViewModel : IViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected object _propertyValueCheckLock = new object();
        public string ErrorMessage { get; set; } = "";
        public bool IsErrorMessageShowed { get; set; } = false;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public virtual void Dispose() { }

        protected async Task RunCommandAsync(bool updatingFlag, Func<Task> action)
        {
            lock (_propertyValueCheckLock)
            {
                if (updatingFlag)
                {
                    return;
                }
                updatingFlag = true;
            }

            try
            {
                await action();
            }
            finally
            {
                updatingFlag = false;
            }
        }

        public void ShowErrorMessage(string message)
        {
            ErrorMessage = message;
            IsErrorMessageShowed = true;
        }
    }
}
