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
    public partial class ForgetPassowrdPage : ContentPage
    {
        ForgetPassowrdPageViewModel ViewModel;
        public ForgetPassowrdPage()
        {
            InitializeComponent();
            ViewModel = BindingContext as ForgetPassowrdPageViewModel;
        }
        private void EntryContentView_Focused(object sender, EventArgs e)
        {
            ShrinkView();
        }

        private void ContentEntry_Unfocused(object sender, EventArgs e)
        {
            ExpandView();
        }

        private void ShrinkView()
        {
            txtContent.Margin = new Thickness(0,20,0,0);
            lgnBtn.Margin = new Thickness(0, 20, 0, 0);
            MainStack.Spacing = 0;
        }

        private void ExpandView()
        {
            txtContent.Margin = new Thickness(0, 100, 0, 0);
            MainStack.Spacing = 40;
        }

    }
}