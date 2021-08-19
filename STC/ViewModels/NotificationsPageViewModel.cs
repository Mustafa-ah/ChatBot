using Prism.Navigation;
using STC.Enums;
using STC.Models;
using STC.Services.Account;
using STC.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace STC.ViewModels
{
    class NotificationsPageViewModel : BaseViewModel
    {
        private readonly IAccountService _accountService;

        private Boolean _NotificationsDeleted;
        public Boolean NotificationsDeleted
        {
            get { return _NotificationsDeleted; }
            set { SetProperty(ref _NotificationsDeleted, value); }
        }
        int AppLang;
        public NotificationsPageViewModel(INavigationService navigationService,
            IAccountService accountService,
            ISettingsService settingsService) : base(navigationService, settingsService)
        {
            _accountService = accountService;
            AppLang = settingsService.AppLanguage;
            NotificationsDeleted = false;

        }
        private ObservableCollection<Notification> ReadyRequests = new ObservableCollection<Notification>();
        public ICommand OnNotificationClicked => new Command<Notification>(OnNotificationClickExecute);


        private ObservableCollection<Notification> _Notifications;
        public ObservableCollection<Notification> Notifications
        {
            get { return _Notifications; }
            set { SetProperty(ref _Notifications, value); }

        }
        public ICommand RemoveNotificationsCommand => new Command(()=> { Notifications.Clear(); NotificationsDeleted = true; });


        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            Setting.HasNewNotifications = false;
            await GetUserNotifications();
        }

        private async Task GetUserNotifications()
        {
            if (!IsConncted())
            {
                return;
            }
            StatusCode requestStatusCode = StatusCode.Ok;
            try
            {
                var request = await _accountService.GetUserNotifications(Setting.AuthAccessToken);
                requestStatusCode = (StatusCode)request.StatusCode;
                foreach (var item in request.Data)
                {
                    if (Lang == (int)Common.Enums.Languages.Arabic)
                    {
                        item.Description = item.DescriptionAr;
                        item.Title = item.TitleAr;
                        item.FormatedDate = string.Format("{0} {1} {2}", item.CreatedAt.Day, item.CreatedAt.ToString("MMMM", new CultureInfo("ar-AE")),item.CreatedAt.Year);
                    }
                    else
                    {
                        item.FormatedDate = string.Format("{0} {1} {2}", item.CreatedAt.Day, item.CreatedAt.ToString("MMMM", new CultureInfo("en-US")), item.CreatedAt.Year);
                    }
                }
                Notifications = new ObservableCollection<Notification>(request.Data);

              
            }
            catch (Exception ex)
            {
               await HandelException(ex, requestStatusCode);
                Console.WriteLine(ex.Message);
            } 
        }
        public async void OnNotificationClickExecute(Notification obj)
        {

        if(obj.NotificationType==2 && obj.RequestId!=null)
            {
                var parameters = new NavigationParameters {
                    { Constants.ParameterKey.RequestId, obj.RequestId },
                            { "Tab","Inquiries"}

                };
                await NavigationService.NavigateAsync(Routes.ViewsRoutes.RequestDetailsRoute, parameters);

            }
        else if(obj.NotificationType == 2 && obj.RequestId == null)
            {
                await NavigationService.NavigateAsync(Routes.ViewsRoutes.InquiriesRoute);
            }
        else if(obj.NotificationType==3 && obj.RequestId!=null)
            {
                var parameters = new NavigationParameters {
                    { Constants.ParameterKey.RequestId, obj.RequestId },
                    { "Tab","Contract"}
              
                };
                await NavigationService.NavigateAsync(Routes.ViewsRoutes.RequestDetailsRoute, parameters);

            }
        }
    }
}
