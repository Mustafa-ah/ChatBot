using System;
using System.Collections.Generic;
using System.Globalization;
using STC.Common.Enums;

namespace STC.Interfaces
{
    public interface IAppCulture
    {
        public void SetAppCulture(Common.Enums.Languages languages);

        public IEnumerable<CultureInfo> GetSupportedCultures();

        public Common.Enums.Languages[] GetLanguagesEnums();
    }
}
