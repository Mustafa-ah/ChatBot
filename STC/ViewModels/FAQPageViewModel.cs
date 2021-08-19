using Prism.Commands;
using Prism.Navigation;
using STC.Models;
using STC.Services.FAQs;
using STC.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace STC.ViewModels
{
    public class FAQPageViewModel : BaseViewModel
    {

     
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


        public override  Task ViewAppeared()
        {
          
           
       
            return base.ViewAppeared();
        }

        private int AppLang;
        private readonly IFAQService _faqservice; 
        public FAQPageViewModel(INavigationService navigationService, IFAQService faqservice, ISettingsService settingsService) : base(navigationService, settingsService)
        {
            AppLang = settingsService.AppLanguage;
            _faqservice = faqservice;
            IsNoData = true;
        }


       
        public async Task<List<FAQDTO>> GetFaAQs()
        {
            try
            {
                var respons = await _faqservice.GetAllFAQS(Setting.AuthAccessToken);

                return respons.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<FAQDTO>();
            }
        }
     
    }
}