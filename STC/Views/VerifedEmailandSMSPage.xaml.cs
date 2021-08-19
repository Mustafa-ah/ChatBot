using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STC.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace STC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerifedEmailandSMSPage : ContentPage
    {
        public VerifedEmailandSMSPage()
        {
            InitializeComponent();
        }
        protected override bool OnBackButtonPressed()
        {
            (BindingContext as VerifedEmailandSMSPageViewModel).OpenLoginPageCommand.Execute(null);
            return base.OnBackButtonPressed();
        }
    }
}