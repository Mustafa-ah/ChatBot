using System;
using System.Collections.Generic;
using STC.ViewModels;
using Xamarin.Forms;

namespace STC.Cells
{
    public partial class SenderCell : ViewCell
    {
        public SenderCell()
        {
            InitializeComponent();
        }
        void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            var model = (sender as Image).BindingContext;
            var viewModel = App.Current.Container.Resolve(typeof( RequestDetailsPageViewModel)) as RequestDetailsPageViewModel;
            viewModel.DownloadChatAttachmentCommand.Execute(model);

        }
    }
}
