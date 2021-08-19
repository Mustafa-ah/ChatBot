using System;
using Prism.Navigation;
using STC.Settings;

namespace STC.ViewModels.DialogsViewModels
{
    public class TermsDialogViewModel : BaseViewModel
    {
        public TermsDialogViewModel(INavigationService navigationService, ISettingsService settingsService) : base(navigationService, settingsService)
        {
        }
    }
}
