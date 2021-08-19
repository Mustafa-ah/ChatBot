using Rg.Plugins.Popup.Pages;
using STC.ViewModels;
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
    public partial class DidntSaveDialog : PopupPage
    {
        DidntSaveDialogViewModel DidntSaveDialogViewModel;
        public DidntSaveDialog()
        {
            InitializeComponent();
            BindingContext = App.Current.Container.Resolve(typeof(DidntSaveDialogViewModel));
            DidntSaveDialogViewModel = BindingContext as DidntSaveDialogViewModel;
            MainView.FlowDirection = DidntSaveDialogViewModel.Lang == (int)Common.Enums.Languages.Arabic ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }
    }
}