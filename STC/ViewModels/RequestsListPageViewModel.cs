using Prism.Navigation;
using STC.Enums;
using STC.Models;
using STC.Services.Requests;
using STC.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace STC.ViewModels
{
    public class RequestsListPageViewModel : BaseViewModel
    {
        private readonly IRequestService _requestService;
        private ObservableCollection<RequestDTO> _Requests;
        public ObservableCollection<RequestDTO> Requests
        {
            get { return _Requests; }
            set { SetProperty(ref _Requests, value);  }

        }

        private bool _isNoData;
        public bool IsNoData
        {
            get
            {
                return _isNoData;
            }
            set
            {
                SetProperty(ref _isNoData, value);
            }
        }
        private bool _isData;
        public bool IsData
        {
            get
            {
                return _isData;
            }
            set
            {
                SetProperty(ref _isData, value);
            }
        }
        #region ctor
        public RequestsListPageViewModel(
          INavigationService navigationService,
          IRequestService requestService,
          ISettingsService settingsService)
          : base(navigationService, settingsService)
        {
            _requestService = requestService;

            IsNoData = true;
            IsData = false;
            //  Lang = settingsService.AppLanguage;
            MessagingCenter.Subscribe<BaseViewModel, Notification>(this, "broadcastnotify", async (sender, arg) =>
            {
               
                if (arg.NotificationType == (int)NotificationType.InProgressRequest || arg.NotificationType == (int)NotificationType.UploadContract || arg.NotificationType == (int)NotificationType.NewRequest)
                {
                    await GetAllRequests();
                }
            });

        }
        #endregion



        #region Commands
        public ICommand OpenRequestDetailsCommand => new Command(OpenRequestDetailsCommandExcute);

        public ICommand OpenNewRequestCommand => new Command(OpenNewRequestCommandExcute);


        private async void OpenRequestDetailsCommandExcute(object obj)
        {
            ShowLoading();

            if (obj is RequestDTO request)
            {
                var parameters = new NavigationParameters { 
                    { Constants.ParameterKey.RequestId, request.id }
                };
                await NavigationService.NavigateAsync(Routes.ViewsRoutes.RequestDetailsRoute, parameters);
            }

            HideLoading();
        }
        #endregion

        #region Methods

        private async void OpenNewRequestCommandExcute(object obj)
        {
            if (IsBusy)
                return;
            ShowLoading();

            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (status != PermissionStatus.Granted)
            {
                await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }

            await NavigationService.NavigateAsync(Routes.ViewsRoutes.NewRequetRoute);

            HideLoading();
        }
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {

            base.OnNavigatedTo(parameters);

            await GetAllRequests();

        }

        private async Task GetAllRequests()
        {
            try
            {
                if (!IsConncted())
                {
                    return;
                }
                ShowLoading();

                var requestsList = await _requestService.GetAllRequests(Setting.AuthAccessToken);

                Requests = new ObservableCollection<RequestDTO>(requestsList.Data);

                for (int i = 0; i < Requests.Count; i++)
                {
   
                    if (Lang == (int)STC.Common.Enums.Languages.Arabic)
                    {
                        Requests[i].requestStatus = Requests[i].requestStatusAr;
                    }
                       
                }
              
                IsNoData = Requests.Count == 0;
                IsData = Requests.Count > 0;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            HideLoading();
        }

        #endregion

    }


}
