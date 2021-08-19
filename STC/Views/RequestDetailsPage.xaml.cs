using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using STC.Dialogs;
using STC.Models;
using STC.TemplateSelector;
using STC.ViewModels;
using Syncfusion.XForms.Border;
using Syncfusion.XForms.Chat;
using Xamarin.Forms;

namespace STC.Views
{
    public partial class RequestDetailsPage : ContentPage
    {
        readonly int x_dir;
        RequestDetailsPageViewModel viewModel;

        public RequestDetailsPage()
        {
            InitializeComponent();
            
            viewModel = BindingContext as RequestDetailsPageViewModel;
            x_dir = viewModel.Lang == (int)Common.Enums.Languages.Arabic ? -1 : 1;
            MessagingCenter.Subscribe<ContractPageViewModel>(this, "OpenInquiriesTab", OnOpenInquiriesTab);

            MessagingCenter.Subscribe<SignaturePageViewModel>(this, "OPenContractTabAfterSign", OPenContractTabAfterSign);

            sfChat.Editor.Focused += Editor_Focused;
            sfChat.Editor.Unfocused += Editor_Unfocused;
            sfChat.MessageTemplate = new InquiriesTemplateSelector(this.sfChat);

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

        private void Editor_Unfocused(object sender, FocusEventArgs e)
        {
            (BindingContext as RequestDetailsPageViewModel).IsKeyboardOpen = e.IsFocused;
        }

        private void Editor_Focused(object sender, FocusEventArgs e)
        {
            ScrollToBottom(sender); 
            (BindingContext as RequestDetailsPageViewModel).IsKeyboardOpen = e.IsFocused;
        }

        private void OPenContractTabAfterSign(SignaturePageViewModel obj)
        {
            (BindingContext as RequestDetailsPageViewModel).ReloadRequest();
            ContractsBtn_Clicked(obj, new EventArgs());

        }
       

        private void OnOpenInquiriesTab(ContractPageViewModel obj)
        {
            InquiriesBtn_Clicked(obj, new EventArgs());
           // RootScroll.ScrollToAsync(0, int.MaxValue, true);
        }
        private void OnOpenInquiriesTabFromNotification(RequestDetailsPageViewModel obj)
        {
            InquiriesBtn_Clicked(obj, new EventArgs());
            //RootScroll.ScrollToAsync(0, int.MaxValue, true);
        }
        private void OnOpenContractTabFromNotification(RequestDetailsPageViewModel obj)
        {
            ContractsBtn_Clicked(obj, new EventArgs());
            //RootScroll.ScrollToAsync(0, int.MaxValue, true);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<RequestDetailsPageViewModel>(this, "OpenInquiriesTab", OnOpenInquiriesTabFromNotification);
            MessagingCenter.Subscribe<RequestDetailsPageViewModel>(this, "OpenContractTab", OnOpenContractTabFromNotification);
            MessagingCenter.Subscribe<RequestDetailsPageViewModel>(this, "toBottom", ScrollToBottom);

            SetEditorStyle();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            MessagingCenter.Unsubscribe<RequestDetailsPageViewModel, bool>(this, "HandleChat");
            MessagingCenter.Unsubscribe<RequestDetailsPageViewModel>(this, "OpenContractTab");
            MessagingCenter.Unsubscribe<RequestDetailsPageViewModel>(this, "OpenInquiriesTab");
            MessagingCenter.Unsubscribe<RequestDetailsPageViewModel>(this, "toBottom");
            MessagingCenter.Send(this, "FocusKeyboardStatus");

            SaveLastInquiry();
        }
        private void ScrollToBottom(object obj)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (this.viewModel.Messages.Count > 0)
                {
                    this.sfChat.ScrollToMessage(this.viewModel.Messages.LastOrDefault());
                }
            });
        }
        private void SaveLastInquiry()
        {
            try
            {
                var list = viewModel.Setting.RequestInquiryMessageList;

                RequestInquiryMessage request = list.FirstOrDefault(r => r.Id == viewModel.UserRequest.id);

                if (request != null)
                {
                    request.Message = viewModel.Message;
                }
                else
                {
                    list.Add(new RequestInquiryMessage() { Id = viewModel.UserRequest.id, Message = viewModel.Message });
                }
                viewModel.Setting.RequestInquiryMessageList = list;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        async void FilesBtn_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                await UnderlineView.TranslateTo(0, 0, 150);
                SetLabelsStyle();
                LblFiles.FontFamily = "STCForwardRegular";
                LblFiles.TextColor = Color.FromHex("#575858");
                HideAll();
                FilesView.IsVisible = true;
                if (BindingContext != null)
                {
                    var item = (BindingContext as RequestDetailsPageViewModel).Files;
                    (BindingContext as RequestDetailsPageViewModel).Files = new ObservableCollection<Attachment>(item);
                }

                MessagingCenter.Send(this, "FocusKeyboardStatus");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
        }

        async void InquiriesBtn_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                await UnderlineView.TranslateTo((x_dir * UnderlineView.Width), 0, 150);
                SetLabelsStyle();
                LblInquiries.FontFamily = "STCForwardRegular";
                LblInquiries.TextColor = Color.FromHex("#575858");
                HideAll();
                InquiriesView.IsVisible = true;
                MessagingCenter.Send(this, "FocusKeyboardStatus");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
          
        }

        async void ContractsBtn_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                await UnderlineView.TranslateTo((UnderlineView.Width * 2 * x_dir), 0, 150);
                SetLabelsStyle();
                LblContract.FontFamily = "STCForwardRegular";
                LblContract.TextColor = Color.FromHex("#575858");
                HideAll();
                ContractsView.IsVisible = true;

                if (BindingContext != null)
                {
                    var item = (BindingContext as RequestDetailsPageViewModel).Contracts;
                    (BindingContext as RequestDetailsPageViewModel).Contracts = new ObservableCollection<Attachment>(item);
                }
                MessagingCenter.Send(this, "FocusKeyboardStatus");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
        }

        private void HideAll()
        {
            FilesView.IsVisible = false;
            InquiriesView.IsVisible = false;
            ContractsView.IsVisible = false;
        }

        private void SetLabelsStyle()
        {
            LblFiles.FontFamily = "STCForwardBold";
            LblFiles.TextColor = Color.FromHex("#8E9AA0");
            LblInquiries.FontFamily = "STCForwardBold";
            LblInquiries.TextColor = Color.FromHex("#8E9AA0");
            LblContract.FontFamily = "STCForwardBold";
            LblContract.TextColor = Color.FromHex("#8E9AA0");
        }


        ~RequestDetailsPage()
        {
          
            MessagingCenter.Unsubscribe<ContractPageViewModel>(this, "OpenInquiriesTab");
            MessagingCenter.Unsubscribe<SignaturePageViewModel>(this, "OPenContractTabAfterSign");
        }

        private async void SfChat_SendMessage(object sender, Syncfusion.XForms.Chat.SendMessageEventArgs e)
        {
            RequestDetailsPageViewModel viewModel = (BindingContext as RequestDetailsPageViewModel);
            viewModel.Message = sfChat.Editor.Text;
           
            e.Handled = true;

            viewModel.SendMessageCommand.Execute(sender);

            await Task.Delay(100);

            sfChat.Editor.Text = string.Empty;
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

                if (viewModel.Lang == (int)STC.Common.Enums.Languages.Arabic)
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

        #region Attach SfPopupLayout
       

       

        private void sfChat_AttachmentButtonClicked(object sender, EventArgs e)
        {
            sfChat.Editor.Unfocus();
            ShowAttachementMenu();
        }

        private async void TapGestureRecognizer_Tapped_ImgImage(object sender, EventArgs e)
        {
            try
            {
                HideAttachementMenu();
                await (BindingContext as RequestDetailsPageViewModel).PickFileAsync(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async void TapGestureRecognizer_Tapped_ImgFile(object sender, EventArgs e)
        {
            try
            {
                HideAttachementMenu();
                await (BindingContext as RequestDetailsPageViewModel).PickFileAsync(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            HideAttachementMenu();
        }

        private void ShowAttachementMenu()
        {
            GridAttachMenu.IsVisible = true;
            PancakeMenu.ScaleTo(1, 150);
        }
        private void HideAttachementMenu()
        {
            PancakeMenu.ScaleTo(0, 150);
            GridAttachMenu.IsVisible = false;
        }
        #endregion


    }
}
