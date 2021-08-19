using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using STC.Common.Enums;
using STC.Interfaces;
using STC.Resources;

namespace STC.Services
{
    public class AppCulture : IAppCulture
    {
        public void SetAppCulture(Languages languages)
        {
            string lang = languages switch
            {
                Languages.Arabic => "ar",
                Languages.English => "en",
                _ => "en"
            };

            CultureInfo culture = new CultureInfo(lang);

            CultureInfo.DefaultThreadCurrentCulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
            CultureInfo.CurrentCulture = culture;

            //Setting.AppLanguage = (int)Common.Enums.Languages.Arabic;

            AppResources.Culture = culture;

        }


        public IEnumerable<CultureInfo> GetSupportedCultures()
        {

            // string currentLanguageTwoLetterCode = ApplicationManager.Instance.GetApplicationInfo().Language.ToDescriptionString();

            Languages[] languagesEnumsArr = GetLanguagesEnums();

            IEnumerable<string> supportedLanguageCodes = languagesEnumsArr.Select(languageEnum => languageEnum.ToString());

            return CultureInfo.GetCultures(CultureTypes.AllCultures)
                                                                    .Where(culture => supportedLanguageCodes.Contains(culture.TwoLetterISOLanguageName) &&
                                                                    culture.Calendar is GregorianCalendar)
                                                                    .GroupBy(culture => culture.TwoLetterISOLanguageName)
                                                                    .Select(cultureGrouping => cultureGrouping.FirstOrDefault());
        }

        public Languages[] GetLanguagesEnums()
        {
            IEnumerable<Languages> languagesEnums = Enum.GetValues(typeof(Languages)).Cast<Languages>();
            return languagesEnums as Languages[] ?? languagesEnums.ToArray();
        }
    }
}