using STC.Models;
using STC.TemplateSelector;
using STC.ViewModels;
using Syncfusion.XForms.Border;
using Syncfusion.XForms.Chat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace STC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewInquiryPage : ContentPage
    {
        NewInquiryPageViewModel PageViewModel;
        public NewInquiryPage()
        {
            InitializeComponent();
            PageViewModel = BindingContext as NewInquiryPageViewModel;
            sfChat.Editor.Completed += Editor_Completed;
            sfChat.MessageTemplate = new InquiriesTemplateSelector(this.sfChat);


            sfChat.Editor.Focused += Editor_Focused;
            sfChat.Editor.TextChanged += Editor_TextChanged;
        }

        private void Editor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sfChat.Editor.Text.Count() == 1)
            {
                if (IsArabicText(e.NewTextValue))
                {
                    sfChat.Editor.FlowDirection = FlowDirection.RightToLeft;
                }
                else
                {
                    sfChat.Editor.FlowDirection = FlowDirection.LeftToRight;
                }
                
            }
        }


        private bool IsArabicText(string text)
        {
            Regex regex = new Regex("[\u0600-\u06ff]|[\u0750-\u077f]|[\ufb50-\ufc3f]|[\ufe70-\ufefc]");
            return regex.IsMatch(text);
        }

        private void Editor_Focused(object sender, FocusEventArgs e)
        {
            ScrollToBottom(sender);
        }
        private void Editor_Completed(object sender, EventArgs e)
        {
            PageViewModel.Message = sfChat.Editor.Text;
        }
        private void ScrollToBottom(object obj)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (this.PageViewModel.Messages.Count > 0)
                {
                    this.sfChat.ScrollToMessage(this.PageViewModel.Messages.LastOrDefault());
                }
            });
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<NewInquiryPageViewModel>(this, "toBottom", ScrollToBottom);
            SetEditorStyle();
            if (string.IsNullOrEmpty(PageViewModel.Setting.GeneralInquiryMessage))
            {
                sfChat.Editor.Text = PageViewModel.Setting.GeneralInquiryMessage;
            }
           
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<NewInquiryPageViewModel>(this, "toBottom");

            PageViewModel.Setting.GeneralInquiryMessage = sfChat.Editor.Text;
        }
        private void SetEditorStyle()
        {
            try
            {

                if (Device.RuntimePlatform == Device.iOS)
                {
                    sfChat.Editor.Placeholder = string.Empty;
                }
                else
                {
                    sfChat.Editor.Placeholder = STC.Resources.AppResources.TypeMessage;
                }
                

                sfChat.Editor.FontFamily = "STCForwardRegular";

                if (PageViewModel.Lang == (int)STC.Common.Enums.Languages.Arabic)
                {
                    var childs = sfChat.Children;
                    var grid = childs[0] as Grid;
                    var gridChilds = grid.Children;
                    FooterView footerView = gridChilds[1] as FooterView;

                    SfBorder border = (footerView.Children[1] as ContentView).Content as SfBorder;

                    Grid subContainerGrid = (border.Content as Grid).Children[0] as Grid;

                    SendMessageView sendMessageView = (border.Content as Grid).Children[2] as SendMessageView;

                    Syncfusion.XForms.EffectsView.SfEffectsView effctsView = sendMessageView.Content as Syncfusion.XForms.EffectsView.SfEffectsView;

                    Grid effctsViewGrid = effctsView.Content as Grid;

                    var image = effctsViewGrid.Children[0].RotationY = 180;

                    //Editor editor = subContainerGrid.Children[0] as Editor;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private async void SfChat_SendMessage(object sender, Syncfusion.XForms.Chat.SendMessageEventArgs e)
        {
            NewInquiryPageViewModel viewModel = (BindingContext as NewInquiryPageViewModel);
            viewModel.Message = sfChat.Editor.Text;

            e.Handled = true;

            viewModel.SendMessageCommand.Execute(sender);

            await Task.Delay(100);
            sfChat.Editor.Text = string.Empty;
        }
    }
}