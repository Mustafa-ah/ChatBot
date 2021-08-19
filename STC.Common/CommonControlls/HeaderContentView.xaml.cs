using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace STC.Common.CommonControlls
{
    public partial class HeaderContentView : ContentView
    {
        public HeaderContentView()
        {
            InitializeComponent();
           // BindingContext = this;
        }

        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(HeaderContentView), null, BindingMode.TwoWay);
        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(string), typeof(HeaderContentView), null, BindingMode.TwoWay);
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(string), typeof(HeaderContentView), null, BindingMode.TwoWay);


        public string Title
        {
            get => (string)this.GetValue(TitleProperty);
            set => this.SetValue(TitleProperty, value);
        }
        public string FontSize
        {
            get => (string)this.GetValue(FontSizeProperty);
            set => this.SetValue(FontSizeProperty, value);
        }
        public string TextColor
        {
            get => (string)this.GetValue(TextColorProperty);
            set => this.SetValue(TextColorProperty, value);
        }


        public static readonly BindableProperty LeadingImageProperty = BindableProperty.Create(nameof(LeadingImage), typeof(string), typeof(HeaderContentView), "ic_back", BindingMode.TwoWay);

        public string LeadingImage
        {
            get => (string)this.GetValue(LeadingImageProperty);
            set => this.SetValue(LeadingImageProperty, value);
        }

        public static readonly BindableProperty TrailingImageProperty = BindableProperty.Create(nameof(TrailingImage), typeof(ImageSource), typeof(HeaderContentView), null, BindingMode.TwoWay);

        public ImageSource TrailingImage
        {
            get => (ImageSource)this.GetValue(TrailingImageProperty);
            set => this.SetValue(TrailingImageProperty, value);
        }


        //public static readonly BindableProperty RightCommandProperty = BindableProperty.Create(nameof(RightCommand), typeof(ICommand), typeof(HeaderContentView), null, BindingMode.TwoWay);

        //public ICommand RightCommand
        //{
        //    get => (ICommand)this.GetValue(RightCommandProperty);
        //    set => this.SetValue(RightCommandProperty, value);
        //}

        // BindableProperty implementation
        public static readonly BindableProperty LangProperty =
            BindableProperty.Create(nameof(Lang), typeof(int), typeof(HeaderContentView), (int)Enums.Languages.English);

        public int Lang
        {
            get { return (int)GetValue(LangProperty); }
            set { SetValue(LangProperty, value); }
        }

        public static readonly BindableProperty LeadingImageCommandProperty =
           BindableProperty.Create(nameof(LeadingImageCommand), typeof(ICommand), typeof(HeaderContentView), null);

        public ICommand LeadingImageCommand
        {
            get { return (ICommand)GetValue(LeadingImageCommandProperty); }
            set { SetValue(LeadingImageCommandProperty, value); }
        }
        public static void Execute(ICommand command)
        {
            if (command == null) return;
            if (command.CanExecute(null))
            {
                command.Execute(null);
            }
        }

        // this is the command that gets bound by the control in the view
        // (ie. a Button, TapRecognizer, or MR.Gestures)
        public Command OnLeadingImageCommand => new Command(() => Execute(LeadingImageCommand));
    }
}
