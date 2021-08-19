
using STC.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace STC.Cells
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReceiverCell : ViewCell
    {
        public ReceiverCell()
        {
            InitializeComponent();
        }

        void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            var model = (sender as Image).BindingContext;
            var viewModel = App.Current.Container.Resolve(typeof(RequestDetailsPageViewModel)) as RequestDetailsPageViewModel;
            viewModel.DownloadChatAttachmentCommand.Execute(model);
        }
    }
}
