using System;
using System.Windows.Input;
using STC.Common.Validations;
using Xamarin.Forms;

namespace STC.Common.CommonControlls
{
    public partial class EntryContentView : ContentView
    {
        public event EventHandler EntryFocused;
        public event EventHandler EntryUnFocused;
        public EntryContentView NextFocus { get; set; }

        public static readonly BindableProperty LeadingImageProperty = BindableProperty.Create(nameof(LeadingImage), typeof(ImageSource), typeof(EntryContentView), null, BindingMode.TwoWay);

        public ImageSource LeadingImage
        {
            get => (ImageSource)this.GetValue(LeadingImageProperty);
            set => this.SetValue(LeadingImageProperty, value);
        }


        public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create(nameof(IsPassword), typeof(bool), typeof(EntryContentView), false, BindingMode.TwoWay);

        public bool IsPassword
        {
            get => (bool)this.GetValue(IsPasswordProperty);
            set => this.SetValue(IsPasswordProperty, value);
        }

        public static readonly BindableProperty TrailingHintProperty = BindableProperty.Create(nameof(TrailingHint), typeof(string), typeof(EntryContentView), null, BindingMode.TwoWay);

        public string TrailingHint
        {
            get => (string)this.GetValue(TrailingHintProperty);
            set => this.SetValue(TrailingHintProperty, value);
        }

        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(EntryContentView), null, BindingMode.TwoWay);

        public string Placeholder
        {
            get => (string)this.GetValue(PlaceholderProperty);
            set => this.SetValue(PlaceholderProperty, value);
        }


        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(string), typeof(EntryContentView), "#5D0095", BindingMode.TwoWay);

        public string TextColor
        {
            get => (string)this.GetValue(TextColorProperty);
            set => this.SetValue(TextColorProperty, value);
        }

        public static readonly BindableProperty PlaceholderColorProperty = BindableProperty.Create(nameof(PlaceholderColor), typeof(string), typeof(EntryContentView), null, BindingMode.TwoWay);

        public string PlaceholderColor
        {
            get => (string)this.GetValue(PlaceholderColorProperty);
            set => this.SetValue(PlaceholderColorProperty, value);
        }

        public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(ValidatableObject<string>), typeof(EntryContentView), null, BindingMode.TwoWay);

        public ValidatableObject<string> Value
        {
            get => (ValidatableObject<string>)this.GetValue(ValueProperty);
            set => this.SetValue(ValueProperty, value);
        }
        //

        public static readonly BindableProperty KeyboardTypeProperty = BindableProperty.Create(nameof(KeyboardType), typeof(Keyboard), typeof(EntryContentView), Keyboard.Default, BindingMode.TwoWay);

        public Keyboard KeyboardType
        {
            get => (Keyboard)this.GetValue(KeyboardTypeProperty);
            set => this.SetValue(KeyboardTypeProperty, value);
        }

        public static readonly BindableProperty IsActiveProperty = BindableProperty.Create(nameof(IsActive), typeof(bool), typeof(EntryContentView), false, BindingMode.TwoWay);

        public bool IsActive
        {
            get => (bool)this.GetValue(IsActiveProperty);
            set => this.SetValue(IsActiveProperty, value);
        }

        public static readonly BindableProperty UnderLinedProperty = BindableProperty.Create(nameof(Underlined), typeof(bool), typeof(EntryContentView), true, BindingMode.TwoWay);

        public bool Underlined
        {
            get => (bool)this.GetValue(UnderLinedProperty);
            set => this.SetValue(UnderLinedProperty, value);
        }

        //TextAlignment
        public static readonly BindableProperty HorizontalTextAlignmentProperty = BindableProperty.Create(nameof(HorizontalTextAlignment), typeof(TextAlignment), typeof(EntryContentView), TextAlignment.Center, BindingMode.TwoWay);
        public TextAlignment HorizontalTextAlignment
        {
            get => (TextAlignment)this.GetValue(HorizontalTextAlignmentProperty);
            set => this.SetValue(HorizontalTextAlignmentProperty, value);
        }

        public ICommand ReturnCommand => new Command(ReturnCommandExcute);

        private void ReturnCommandExcute(object obj)
        {
            if (NextFocus != null)
            {
                NextFocus.EntryControll.Focus();
            } 
        }

        public static readonly BindableProperty TextChangedCommandProperty =
         BindableProperty.Create(nameof(TextChangedCommand), typeof(ICommand), typeof(EntryContentView), null);

        public ICommand TextChangedCommand
        {
            get { return (ICommand)GetValue(TextChangedCommandProperty); }
            set { SetValue(TextChangedCommandProperty, value); }
        }
        public static void ExecuteCommand(ICommand command)
        {
            if (command == null) return;
            if (command.CanExecute(null))
            {
                command.Execute(null);
            }
        }

        public EntryContentView()
        {
            InitializeComponent();
            //BindingContext = this;
        }

        void EntryControll_Focused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            IsActive = true;
            EntryFocused?.Invoke(sender, e);
        }

        void EntryControll_Unfocused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            IsActive = false;
            EntryUnFocused?.Invoke(sender, e);
        }

        private void MainEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExecuteCommand(TextChangedCommand);
        }

    }
}
