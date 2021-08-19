using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Navigation;
using STC.Models;
using STC.Services.Contract;
using STC.Settings;
using Xamarin.Forms;

namespace STC.ViewModels
{
    public class ContractPageViewModel : BaseViewModel
    {
        private readonly IContractService _contractService;
        public Func<Task<string>> SignatureFromStream { get; set; }
        public string RequestId { get; set; }

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

        private bool _canSign;

        public bool CanSign
        {
            get { return _canSign; }
            set { SetProperty(ref _canSign, value); }
        }


        private string _contractURL;
        public string ContractURL
        {
            get
            {
                return _contractURL;
            }
            set
            {
                SetProperty(ref _contractURL, value);
            }
        }
        public ContractPageViewModel(INavigationService navigationService,
            IContractService contractService,
            ISettingsService settingsService) : base(navigationService, settingsService)
        {
            _contractService = contractService;
            ContractURL = "https://drive.google.com/file/d/0BziB7kxHIi8yc3RhcnRlcl9maWxl/view?usp=sharing";// "http://www.africau.edu/images/default/sample.pdf";
          //  IsBusy = true;
        }

        public ICommand OpenSignaturePageCommand => new Command(OpenSignaturePageCommandExcute);

        private async void OpenSignaturePageCommandExcute(object obj)
        {
            try
            {
                if (!IsConncted())
                {
                    return;
                }
                ShowLoading();
                var request = await _contractService.IsFinalRequestContract(Contract.Id, Contract.RequestId, Contract.VersionNumber, Setting.AuthAccessToken);

                if (!request.Data)
                {
                    ShowErrorToast(request.Message);
                    MessagingCenter.Send(this, "UpdateContract");
                    return;
                }
                if (request.Data)
                {
                    var parameters = new NavigationParameters { 
                        { Constants.ParameterKey.Contract, Contract },
                         { Constants.ParameterKey.RequestId,RequestId }
                    };
                    await NavigationService.NavigateAsync(Routes.ViewsRoutes.SignatureRoute, parameters);
                }
                else
                {
                    ShowWarningToast( Resources.AppResources.ContractUpdated);
                }
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


        public ICommand OpenInquiriesPageCommand => new Command(OpenInquiriesPageCommandExcute);

        private async void OpenInquiriesPageCommandExcute(object obj)
        {
            await NavigationService.GoBackAsync();
            MessagingCenter.Send(this, "OpenInquiriesTab");
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey(Constants.ParameterKey.RequestStatusId))
            {
                CanSign = (int)parameters[Constants.ParameterKey.RequestStatusId] != (int)Enums.RequestStatus.Signed;
            }

            if (parameters.ContainsKey(Constants.ParameterKey.Contract))
            {
                Contract = parameters[Constants.ParameterKey.Contract] as Attachment;

                // ContractURL = Contract.FilePath;

                //Signed = Contract.

                MessagingCenter.Send<ContractPageViewModel>(this, "OnNavigatedTo");

                await  UpdateContractViewState();
            }

            if (parameters.ContainsKey(Constants.ParameterKey.RequestId))
            {
                RequestId = parameters[Constants.ParameterKey.RequestId] as string;
            }
        }


        private async Task UpdateContractViewState()
        {
            try
            {
                 await _contractService.UpdateRequestContractViewState(Contract.Id, true, Setting.AuthAccessToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
