using STC.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace STC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignaturePage : ContentPage
    {
        SignaturePageViewModel ViewModel;

        public SignaturePage()
        {
            InitializeComponent();
            ViewModel = BindingContext as SignaturePageViewModel;

            RegisterPadView_StrokeCompleted();
        }

        private void RegisterPadView_StrokeCompleted()
        {
            ViewModel.SignatureFromStream = async () =>
            {
                if (PadView.Points.Count() > 0)
                {
                    return await PadView.GetImageStreamAsync(SignaturePad.Forms.SignatureImageFormat.Png);
                }
                return await Task.Run(() => (Stream)null);
            };
        }

        //protected override void OnBindingContextChanged()
        //{
        //    base.OnBindingContextChanged();

        //    ViewModel.SignatureFromStream = async () =>
        //    {
        //        if (PadView.Points.Count() > 0)
        //        {
        //            return await PadView.GetImageStreamAsync(SignaturePad.Forms.SignatureImageFormat.Png);
        //        }
        //        return await Task.Run(() => (Stream)null);
        //    };
        //}

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (width > height)
            {
                PadView.VerticalOptions = LayoutOptions.Center;
                PadView.HeightRequest = 100;
            }
            else
            {
                PadView.VerticalOptions = LayoutOptions.FillAndExpand;
            }
        }
    }
}