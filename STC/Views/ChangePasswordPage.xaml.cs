using STC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace STC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangePasswordPage : ContentPage
    {
        ChangePasswordPageViewModel ChangePasswordPageViewModel; 
        public ChangePasswordPage()
        {
            InitializeComponent();
            ChangePasswordPageViewModel = BindingContext as ChangePasswordPageViewModel;
        }

        

       
    }
}