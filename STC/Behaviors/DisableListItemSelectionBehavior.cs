using System;
using Xamarin.Forms;

namespace STC.Behaviors
{
    public class DisableListItemSelectionBehavior : Behavior<ListView>
    {
        protected override void OnAttachedTo(ListView bindable)
        {
            base.OnAttachedTo(bindable);

            if (Device.RuntimePlatform == Device.Android)
            {
                bindable.ItemSelected += ListViewOnItemSelected;
            }
        }

        protected override void OnDetachingFrom(ListView bindable)
        {
            base.OnDetachingFrom(bindable);

            if (Device.RuntimePlatform == Device.Android)
            {
                bindable.ItemSelected -= ListViewOnItemSelected;
            }
        }

        private void ListViewOnItemSelected(object sender, SelectedItemChangedEventArgs selectedItemChangedEventArgs)
        {
            ((ListView)sender).SelectedItem = null;
        }
    }
}
