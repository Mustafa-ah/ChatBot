using Xamarin.Forms;

namespace STC.Common.CommonControlls
{
    public partial class BasePageContentView : ContentView
    {
        public View AppConntentView
        {
            get => this.ViewContainer;
            set => this.ViewContainer.Content = value;
        }

        public BasePageContentView()
        {
            InitializeComponent();
        }
    }
}
