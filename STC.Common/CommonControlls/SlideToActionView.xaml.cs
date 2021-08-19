using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace STC.Common.CommonControlls
{
    public partial class SlideToActionView : ContentView
    {
        public SlideToActionView()
        {
            InitializeComponent();
        }

        //
        void Handle_SlideCompleted(object sender, System.EventArgs e)
        {
            Execute(SlideCompletedCommand);
        }


        public static readonly BindableProperty SliderLabelProperty = BindableProperty.Create(nameof(SliderLabel), typeof(string), typeof(SlideToActionView), null);

        public string SliderLabel
        {
            get => (string)this.GetValue(SliderLabelProperty);
            set => this.SetValue(SliderLabelProperty, value);
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == nameof(SliderLabel))
            {
                TrackLabel.Text = SliderLabel;
            }
            
        }

        public static readonly BindableProperty SlideCompletedCommandProperty =
          BindableProperty.Create(nameof(SlideCompletedCommand), typeof(ICommand), typeof(SlideToActionView), null);

        public ICommand SlideCompletedCommand
        {
            get { return (ICommand)GetValue(SlideCompletedCommandProperty); }
            set { SetValue(SlideCompletedCommandProperty, value); }
        }
        public static void Execute(ICommand command)
        {
            if (command == null) return;
            if (command.CanExecute(null))
            {
                command.Execute(null);
            }
        }

    }
}
