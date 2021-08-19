using Prism.Navigation;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using STC.ViewModels.DialogsViewModels;
using STC.ViewModels.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace STC.Dialogs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgetPassowrdDialog : PopupPage
    {
        ForgetPassowrdDialogViewModel DialogViewModel;

        public ForgetPassowrdDialog()
        {
            InitializeComponent();

            BindingContext = App.Current.Container.Resolve(typeof(ForgetPassowrdDialogViewModel));
            DialogViewModel = BindingContext as ForgetPassowrdDialogViewModel;

            MainView.FlowDirection = DialogViewModel.Lang == (int)Common.Enums.Languages.Arabic ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);

            PopupNavigation.Instance.Popped += Instance_Popped;
        }

        private void SaveBtn_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
            //DialogViewModel.NavigationService.NavigateAsync(ViewsRoutes.ForgetPasswordRoute);

        }

        private async void Instance_Popped(object sender, Rg.Plugins.Popup.Events.PopupNavigationEventArgs e)
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                PopupNavigation.Instance.Popped -= Instance_Popped;
            }
            await DialogViewModel.NavigationService.NavigateAsync(ViewsRoutes.ForgetPasswordRoute);
        }
        protected override bool OnBackButtonPressed()
        {
            PopupNavigation.Instance.Popped -= Instance_Popped;
            return base.OnBackButtonPressed();
        }
    }
}