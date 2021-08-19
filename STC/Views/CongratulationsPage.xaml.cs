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
    public partial class CongratulationsPage : ContentPage
    {
        public CongratulationsPage()
        {
            InitializeComponent();
        }
        protected override bool OnBackButtonPressed()
        {
            (BindingContext as CongratulationsPageViewModel).OpenLoginPageCommand.Execute(null);
            return true;
        }
    }
}