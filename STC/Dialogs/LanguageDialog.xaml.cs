using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using STC.ViewModels.DialogsViewModels;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace STC.Dialogs
{
    public partial class LanguageDialog : PopupPage
    {

        LanguageDialogViewModel DialogViewModel;
        public LanguageDialog()
        {
            InitializeComponent();

            BindingContext = App.Current.Container.Resolve(typeof(LanguageDialogViewModel));

            DialogViewModel = BindingContext as LanguageDialogViewModel;

            MainView.FlowDirection = DialogViewModel.Lang == (int)Common.Enums.Languages.Arabic ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;

            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }
    }
}
