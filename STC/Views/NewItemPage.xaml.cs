using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using STC.Models;

namespace STC.Views
{
    public partial class NewItemPage : ContentPage, INotifyPropertyChanged
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();

            Item = new Item
            {
                Text = "Item name",
                Description = "This is an item description."
            };

            BindingContext = this;

            MyTitle = "binding from common";
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddItem", Item);
            await Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            MyTitle = "PopModalAsync PopModalAsync";
           // await Navigation.PopModalAsync();
        }

        private string ss;
        public string MyTitle
        {
            get => ss;
            set {

                ss = value;
                OnPropertyChanged();
            }
        }
    }
}