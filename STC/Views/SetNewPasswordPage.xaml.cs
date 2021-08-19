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
    public partial class SetNewPasswordPage : ContentPage
    {
        public SetNewPasswordPage()
        {
            InitializeComponent();
        }
        protected override bool OnBackButtonPressed()
        {
            (BindingContext as SetNewPasswordPageViewModel).OpenLoginPageCommand.Execute(null);
            return true;
        }
    }
}