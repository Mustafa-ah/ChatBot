using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace STC.Common.CommonControlls
{
    public partial class HeaderContentView : ContentView
    {
        public HeaderContentView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(HeaderContentView), null, BindingMode.TwoWay);


        public string Title
        {
            get => (string)this.GetValue(TitleProperty);
            set => this.SetValue(TitleProperty, value);
        }
    }
}
