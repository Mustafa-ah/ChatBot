using Prism.Navigation;
using STC.Settings;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace STC.ViewModels
{
    class CongratulationsPageViewModel : BaseViewModel
    {
        private string _message;
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                SetProperty(ref _message, value);
            }
        }
        public CongratulationsPageViewModel(INavigationService navigationService, ISettingsService settingsService) : base(navigationService, settingsService)
        {
            ViewRoute = Routes.ViewsRoutes.LoginRoute;
        }

        public ICommand OpenLoginPageCommand => new Command(OpenLoginRoute);

        private async void OpenLoginRoute()
        {
            ShowLoading();
            await Task.Delay(500);
            await NavigationService.NavigateAsync($"{ViewRoute}");
            HideLoading();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            SetRoute(parameters);
            if (parameters.ContainsKey("Message"))
            {
                Message = parameters["Message"].ToString();
            }
        }
    }
}
