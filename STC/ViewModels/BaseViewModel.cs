using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xamarin.Forms;

using STC.Models;
using STC.Services;
using Prism.Mvvm;
using Prism.Navigation;
using System.Threading.Tasks;
using System.Windows.Input;
using STC.Settings;
using Microsoft.AspNetCore.SignalR.Client;
using STC.Enums;
using STC.Interfaces;
using Acr.UserDialogs;
using System.Threading;
using Xamarin.Essentials;
using STC.Views;
using AiForms.Dialogs;
using STC.Toasters;
using STC.Common.Enums;
using STC.Resources;

namespace STC.ViewModels
{
    public class BaseViewModel : BindableBase, INavigationAware
    {

        string _title;
        bool _isBusy;
        int _lang;
        string _busyMessage;
        bool _connected;
        string _noConnectionMessage;
        public INavigationService NavigationService  { get; set; }
        public ISettingsService Setting { get; set; }
        public string ViewRoute { get; set; }

        public bool AppInBackground { get; set; }

        //public StatusCode RequestStatusCode { get; set; }
        //ISettingsService

        public BaseViewModel(INavigationService navigationService, ISettingsService settingsService)
        {
            NavigationService = navigationService;
            Setting = settingsService;

            Console.WriteLine($".........................{this.GetType().Name}..........................................");

            Lang = Setting.AppLanguage;
            NoConnectionMessage = AppResources.NotConnected;
            
            BusyMessage = Resources.AppResources.Wait;

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            Connected = true;
            IsConncted();
         
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess == NetworkAccess.Internet)
            {
                Connected = true;
            }
            else
            {
                Connected = false;
            }
        }
        public ICommand BackCommand => new Command(
            () =>
            {
                try
                {
                    var pat = NavigationService.GetNavigationUriPath();
                    var item = NavigationService.GoBackAsync();
                    MessagingCenter.Send(this, "FocusKeyboardStatus");
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
  
            }
            );

        //https://www.c-sharpcorner.com/article/xamarin-forms-getting-starting-with-prism/
        //public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();

        public ICommand DismissNoConnectionViewCommand => new Command(() => Connected = true);

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public bool Connected
        {
            get => this._connected;
            set => this.SetProperty(ref this._connected, value);
        }


        public bool IsBusy
        {
            get => this._isBusy;
            set => this.SetProperty(ref this._isBusy, value);
        }
        public string BusyMessage
        {
            get => this._busyMessage;
            set => this.SetProperty(ref this._busyMessage, value);
        }
        public string NoConnectionMessage
        {
            get => this._noConnectionMessage;
            set => this.SetProperty(ref this._noConnectionMessage, value);
        }
        public int Lang
        {
            get => this._lang;
            set  {
                this.SetProperty(ref this._lang, value);
                   }
        }
        //protected bool SetProperty<T>(ref T backingStore, T value,
        //    [CallerMemberName] string propertyName = "",
        //    Action onChanged = null)
        //{
        //    if (EqualityComparer<T>.Default.Equals(backingStore, value))
        //        return false;

        //    backingStore = value;
        //    onChanged?.Invoke();
        //    OnPropertyChanged(propertyName);
        //    return true;
        //}

        //#region INotifyPropertyChanged
        //public event PropertyChangedEventHandler PropertyChanged;
        //protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        //{
        //    var changed = PropertyChanged;
        //    if (changed == null)
        //        return;

        //    changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}


        //#endregion

        public void HideLoading()
        {
            this.IsBusy = false;
            this.BusyMessage = null;
        }
        public void ShowLoading(string message = null)
        {

            this.IsBusy = true;
            this.BusyMessage = message;
        }
        private  void ShowToast(string message,Color backgroundColor)
        {
           
            Toast.Instance.Show<ToasterView>(new 
            { 
                Message = message, Duration = 5000, 
                ToastBackgroundColor= backgroundColor
            });

        }
        public void ShowSucessToast(string message)
        {
            Color backgroundColor =  Color.FromHex("#00C48C");
            ShowToast(message, backgroundColor);
        }
        public void ShowErrorToast(string message)
        {
            Color backgroundColor = Color.FromHex("#cc0000");
            ShowToast(message, backgroundColor);
        }
        public void ShowWarningToast(string message)
        {
            Color backgroundColor = Color.FromHex("#ffcc00");
            ShowToast(message, backgroundColor);
        }
        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
            //throw new NotImplementedException();
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
            //throw new NotImplementedException();
        }

        public virtual Task ViewAppeared()
        {
            return Task.FromResult(0);
        }


        public bool IsConncted()
        {
            Connected = Connectivity.NetworkAccess == NetworkAccess.Internet;
            NoConnectionMessage = AppResources.NotConnected;
            return Connected;
        }

        #region SignalR 
      
       
        private HubConnection replyHubConnection;
        private HubConnection notificationHubConnection;
        private Timer SignalRTimer;
        public async Task ConnectSignalRInquiryHub()
        {
            try
            {
                string hubURL = $"{BaseService.BaseHubUrl}reply";
                replyHubConnection = new HubConnectionBuilder().WithUrl(hubURL, options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(Setting.AuthAccessToken);
                }).Build();


                replyHubConnection.On<InquiryDTO>("broadcastreply", (message) =>
                {
                    if (message.UserType == UserType.BusinessUser.ToString())
                    {
                        MessagingCenter.Send<BaseViewModel, InquiryDTO>(this, "broadcastreply", message);
                    }
                    
                });
                this.replyHubConnection.ServerTimeout = TimeSpan.FromMilliseconds(3600000);

                this.replyHubConnection.Closed += ReplyHubConnection_Closed;

                await replyHubConnection.StartAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private Task ReplyHubConnection_Closed(Exception arg)
        {
            return RestartReplayConnection();
        }

        public void CreateSignalRTimer()
        {
            SignalRTimer = new Timer(async (e) => {
                if (Connected)
                {
                    await RestartReplayConnection();

                    await RestartNotificationConnection();
                }
               
                Console.WriteLine(" ..................................................SignalR Timer Running..............................");

            }, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(50));
        }
        public async Task ConnectSignalRNotificationsHub()
        {
            try
            {
                string hubURL = $"{BaseService.BaseHubUrl}notify";
                notificationHubConnection = new HubConnectionBuilder().WithUrl(hubURL, options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(Setting.AuthAccessToken);
                }).Build();


                notificationHubConnection.On<Notification>("broadcastnotify", (message) =>
                {
                    if(Setting.TempNotificationId==message.Id)
                    {
                        return;
                    }
                    Setting.TempNotificationId = message.Id;
                    MessagingCenter.Send<BaseViewModel, Notification>(this, "broadcastnotify", message);

                    if (!AppInBackground)
                    {
                        if (Setting.AppLanguage == 1)
                            ShowSucessToast(message.DescriptionAr);
                        if (Setting.AppLanguage == 2)
                            ShowSucessToast(message.Description);
                    }
                  
                    // Setting.HasNewNotifications = true;
                });
                this.notificationHubConnection.ServerTimeout = TimeSpan.FromMilliseconds(3600000);

                this.notificationHubConnection.Closed += NotificationHubConnection_Closed;

                await notificationHubConnection.StartAsync();
         
                // await hubConnection.InvokeAsync("JoinChat", Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private Task NotificationHubConnection_Closed(Exception arg)
        {
           return RestartNotificationConnection();
        }

        private async Task RestartNotificationConnection()
        {
            try
            {
                if (this.notificationHubConnection != null && this.notificationHubConnection.State == HubConnectionState.Disconnected)
                {
                    await this.notificationHubConnection.StartAsync();
                }
               
            }
            catch
            {

            }
        }
        private async Task RestartReplayConnection()
        {
            try
            {
                if (this.replyHubConnection != null && this.replyHubConnection.State == HubConnectionState.Disconnected)
                {
                    await this.replyHubConnection.StartAsync();
                }
               
            }
            catch
            {

            }
        }


        async Task DisconnectReply()
        {
            try
            {
                await replyHubConnection.StopAsync();
            }
            catch
            {

            }
           
        }
        async Task DisconnectNotify()
        {
            try
            {
                await notificationHubConnection.StopAsync();
            }
            catch
            {

            }
           
        }

        #endregion

        #region Set route Navigation
        public void SetRoute(INavigationParameters parameters)
        {
            if (parameters.TryGetValue<string>(Constants.ParameterKey.ViewRoute, out string route))
            {
                ViewRoute = route;
            }
        }
        #endregion

        #region Handel request Status Code
        public async Task HandelException(Exception ex, StatusCode statusCode)
        {
            switch (statusCode)
            {
                case StatusCode.Ok:
                    break;
                case StatusCode.Unauthorized:
                    ShowLoading();
                    await ForceLogout();
                    break;
                default:
                    break;
            }
        }

        private Task ForceLogout()
        {
            Setting.AuthAccessToken = string.Empty;
            Setting.UserId = string.Empty;
            Setting.GeneralInquiryId = string.Empty;
            return NavigationService.NavigateAsync($"/{Routes.ViewsRoutes.LoginRoute}");
        }
        #endregion
    }
}
