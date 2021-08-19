using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using STC.Services;
using STC.Views;
using Prism.Unity;
using Prism.Ioc;
using Prism;
using STC.ViewModels;
using STC.ViewModels.Routes;
using STC.ViewModels.DialogsViewModels;
using STC.Dialogs;

[assembly: ExportFont("STCForward-Bold.otf", Alias = "STCForwardBold")]
[assembly: ExportFont("STCForward-BoldItalic.otf", Alias = "STCForwardBoldItalic")]
[assembly: ExportFont("STCForward-ExtraBold.otf", Alias = "STCForwardExtraBold")]
[assembly: ExportFont("STCForward-ExtraBoldItalic.otf", Alias = "STCForwardExtraBoldItalic")]
[assembly: ExportFont("STCForward-Italic.otf", Alias = "STCForwardItalic")]
[assembly: ExportFont("STCForward-Light.otf", Alias = "STCForwardLight")]
[assembly: ExportFont("STCForward-LightItalic.otf", Alias = "STCForwardLightItalic")]
[assembly: ExportFont("STCForward-Medium.otf", Alias = "STCForwardMedium")]
[assembly: ExportFont("STCForward-MediumItalic.otf", Alias = "STCForwardMediumItalic")]
[assembly: ExportFont("STCForward-Regular.otf", Alias = "STCForwardRegular")]
[assembly: ExportFont("STCForward-Thin.otf", Alias = "STCForwardThin")]
[assembly: ExportFont("STCForward-ThinItalic.otf", Alias = "STCForwardThinItalic")]
namespace STC
{
    public partial class App : PrismApplication
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
          //  MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        public App(IPlatformInitializer platformInitializer = null) : base(platformInitializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();
            //NavigationService.NavigateAsync(PageConstants.MY_PAGE);

            MainPage = new FAQ();
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<AboutPage, AboutViewModel>(ViewsRoutes.HomeRoute);
            containerRegistry.RegisterForNavigation<Contactus, ContactusViewModel>(ViewsRoutes.HomeRoute);
            containerRegistry.RegisterForNavigation<FAQ, FAQViewModel>(ViewsRoutes.HomeRoute);

            //containerRegistry.RegisterForNavigation<MyPage, MyPageViewModel>();
            containerRegistry.RegisterDialog<TermsDialog, TermsDialogViewModel>(ViewsRoutes.LogineRoute);
            

        }

    }
}
