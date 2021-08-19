using Acr.UserDialogs;
using Prism.Navigation;
using STC.Models;
using STC.Services.Contract;
using STC.Settings;
using STC.ViewModels.Routes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace STC.ViewModels
{
    class SignaturePageViewModel : BaseViewModel
    {
        private readonly IContractService _contractService;
        public SignaturePageViewModel(INavigationService navigationService,
            IContractService contractService,
            ISettingsService settingsService) : base(navigationService, settingsService)
        {
            _contractService = contractService;
            TermsAccepted = true;
        }

        public string RequestId { get; set; }

        private bool _termsAccepted;
        public bool TermsAccepted
        {
            get { return _termsAccepted; }
            set { SetProperty(ref _termsAccepted, value); }
        }
        public Func<Task<Stream>> SignatureFromStream { get; set; }

        public ICommand SlideCompletedCommand => new Command(SlideCompleted);

        private async void SlideCompleted(object obj)
        {
            if (!IsConncted())
            {
                return;
            }
            try
            {
                if (!TermsAccepted)
                {
                    ShowMssage(Resources.AppResources.AcceptTerms);
                    return;
                }

                var checkRequest = await _contractService.IsFinalRequestContract(Contract.Id, Contract.RequestId, Contract.VersionNumber, Setting.AuthAccessToken);

                if (!checkRequest.Data)
                {
                    ShowErrorToast(checkRequest.Message);
                    MessagingCenter.Send(this, "UpdateContract");
                    return;
                }

                ShowLoading(Resources.AppResources.Uploading);

                Stream stream = await SignatureFromStream();

                if (stream != null)
                {
                    var request = await _contractService.SignRequestContract(Contract.RequestId, stream, "ContractSignature.png", Setting.AuthAccessToken);

                    var parameters = new NavigationParameters {
                    { Constants.ParameterKey.RequestId, RequestId}
                };

                    var naved = await NavigationService.NavigateAsync($"/{ViewsRoutes.RequestDetailsRoute}", parameters);
                    // for IOS
                    if (!naved.Success)
                    {
                        await NavigationService.NavigateAsync($"/{ViewsRoutes.RequestDetailsRoute}", parameters);
                    }
                    await Task.Delay(2000);
                    MessagingCenter.Send(this, "OPenContractTabAfterSign");
                }
                else
                {
                    ShowErrorToast( Resources.AppResources.MustSign);
                }

                HideLoading();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                HideLoading();
            }
           
        }

        public void ShowMssage(string message)
        {
            var config = new ToastConfig(message)
            {
                Message = message,
                MessageTextColor = Color.White,
                BackgroundColor = Color.Red
            };
            //config.Duration = TimeSpan.FromSeconds(30);
            //config.Icon = "info.png";
            UserDialogs.Instance.Toast(config);
        }

        private Attachment _contract;
        public Attachment Contract
        {
            get
            {
                return _contract;
            }
            set
            {
                SetProperty(ref _contract, value);
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey(Constants.ParameterKey.Contract))
            {
                Contract = parameters[Constants.ParameterKey.Contract] as Attachment;
            }

            if (parameters.ContainsKey(Constants.ParameterKey.RequestId))
            {
                RequestId = parameters[Constants.ParameterKey.RequestId] as string;
            }
        }
    }
}
