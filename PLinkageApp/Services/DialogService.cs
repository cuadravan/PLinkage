using PLinkageApp.Interfaces;

namespace PLinkageApp.Services
{
    public class DialogService : IDialogService
    {
        public Task ShowAlertAsync(string title, string message, string cancel)
        {
            return Shell.Current.DisplayAlert(title, message, cancel);
        }

        public Task<bool> ShowConfirmationAsync(string title, string message, string accept, string cancel)
        {
            return Shell.Current.DisplayAlert(title, message, accept, cancel);
        }
    }
}
