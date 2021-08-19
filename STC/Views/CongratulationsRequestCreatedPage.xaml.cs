using STC.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace STC.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CongratulationsRequestCreatedPage : ContentPage
	{
		public CongratulationsRequestCreatedPage ()
		{
			InitializeComponent ();
		}

		protected override bool OnBackButtonPressed()
		{
			(BindingContext as CongratulationsRequestCreatedPageViewModel)?.CloseCommand.Execute(null);
			return true;
		}
	}
}