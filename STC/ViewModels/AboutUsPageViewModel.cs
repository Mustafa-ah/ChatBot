using Prism.Navigation;
using STC.Services.APPStaticData;
using STC.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace STC.ViewModels
{
    class AboutUsPageViewModel : BaseViewModel
    {
        private readonly IAPPStaticDataService _aboutUsService;
        private string _title;
        public string Tiltle
        {
            get
            {
                return _title;
            }
            set
            {
                SetProperty(ref _title, value);
            }
        }
        private string _description;
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                SetProperty(ref _description, value);
            }
        }
        private int AppLang;
        public AboutUsPageViewModel(IAPPStaticDataService aboutUsService, INavigationService navigationService, ISettingsService settingsService) : base(navigationService, settingsService)
        {
            _aboutUsService = aboutUsService;
            AppLang = settingsService.AppLanguage;
            AboutUs();
        }
        public  void AboutUs()
        {
            Task.Run(async () => {


                try
                {
                    var aboutUsResult = await _aboutUsService.AboutAs(Setting.AuthAccessToken);
                    if (aboutUsResult.StatusCode == 200)
                    {
                        if (AppLang == 2)
                        {
                            Title = aboutUsResult.Data[0].title;
                            Description = aboutUsResult.Data[0].text;
                        }
                        else
                        {
                            Title = aboutUsResult.Data[0].titleAr;
                            Description = aboutUsResult.Data[0].textAr;
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }


            });

        }
    }
}
