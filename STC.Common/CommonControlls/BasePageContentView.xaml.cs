using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace STC.Common.CommonControlls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
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

        public static readonly BindableProperty LangProperty =
           BindableProperty.Create(nameof(Lang), typeof(int), typeof(BasePageContentView), (int)Enums.Languages.English);

        public int Lang
        {
            get { return (int)GetValue(LangProperty); }
            set { SetValue(LangProperty, value); }
        }
    }
}
