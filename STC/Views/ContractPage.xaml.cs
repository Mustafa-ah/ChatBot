using STC.Interfaces;
using STC.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace STC.Views
{
    public partial class ContractPage : ContentPage
    {
        public ContractPageViewModel ViewModel { get; set; }

        Stream stream;

        public ContractPage()
        {
            InitializeComponent();
            ViewModel = this.BindingContext as ContractPageViewModel;
        }

        private void OnNavigatedTo(ContractPageViewModel obj)
        {
            DisplyContract();
        }

        void WebView_Navigated(System.Object sender, Xamarin.Forms.WebNavigatedEventArgs e)
        {
            
           // ViewModel.IsBusy = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<ContractPageViewModel>(this, "OnNavigatedTo", OnNavigatedTo);
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<ContractPageViewModel>(this, "OnNavigatedTo");
            stream?.Dispose();
        }

        private async void DisplyContract()
        {
            try
            {

                IFileHelper _fileService = DependencyService.Get<IFileHelper>();

                string directoryPath = _fileService.GetFilePath("STC");
               // string directoryPath = "";
                ViewModel = this.BindingContext as ContractPageViewModel;

                ViewModel.ShowLoading();

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                string filepath = Path.Combine(directoryPath, ViewModel.Contract.FileName);

                if (!File.Exists(filepath))
                {
                    File.WriteAllBytes(filepath, ViewModel.Contract.DataArray);

                    await Task.Delay(1000);
                }
                   
                 stream = _fileService.OpenStream(filepath, 22);
                pdfViewerControl.LoadDocument(stream);
                ViewModel.HideLoading();
            }
            catch (Exception ex)
            {
                ViewModel.HideLoading();
                Console.WriteLine(ex.Message);
            }
           finally
            {
                
            }
           // PdfDocView.Uri = filepath;
        }
    }
}
