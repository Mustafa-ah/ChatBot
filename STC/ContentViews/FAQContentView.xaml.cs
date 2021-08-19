using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using STC.Models;
using STC.ViewModels;
using Syncfusion.XForms.Expander;
using Xamarin.Forms;

namespace STC.ContentViews
{
    public partial class FAQContentView : ContentView
    {
        public FAQPageViewModel ViewModel { get; set; }
        public List<Models.FAQDTO> FAQsSourse { get; set; }
        public FAQContentView()
        {
            InitializeComponent();
            ViewModel = this.BindingContext as FAQPageViewModel;

            GetFAQs();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (Device.RuntimePlatform == Device.iOS)
            {
                if (width > height)
                {
                    RootGrid.Margin = new Thickness(100, 5, 100, 30);
                }
                else
                {
                    RootGrid.Margin = new Thickness(20, 80, 20, 20);
                }
            }
        }

        void BorderlessEntry_Focused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
        }

        private async Task GetFAQs()
        {
            try
            {
                await Task.Delay(1000);

                ViewModel = this.BindingContext as FAQPageViewModel;

                var lis = FAQsSourse = await ViewModel.GetFaAQs();

                DrawFAQsList(lis);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private void DrawFAQsList(IEnumerable<FAQDTO> faqs)
        {

            Expanders.Children.Clear();

            bool isAR = ViewModel.Lang == (int)Common.Enums.Languages.Arabic;

            foreach (var faq in faqs)
            {
                string question = isAR ? faq.questionAr : faq.question;
                string answer = isAR ? faq.answerAr : faq.answer;

                AddExpander(question, answer);
                AddSeparator();
            }

            ViewModel.IsNoData = faqs.Count() == 0;
        }

        SfExpander PreExpander;
        private void AddExpander(string question, string answer)
        {
            //Expanders

            SfExpander expander = new SfExpander();


            Color backColor = (Color)App.Current.Resources["HomeTabsBackgroundColor"] ;
            StackLayout stackLayoutHeader = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                BackgroundColor = backColor
            };

            var headerLabel = new Label()
            {
                FontFamily = "STCForwardBold",
                FontSize = 17,
                HeightRequest = 50,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = question,
                TextColor = Color.White,
                VerticalTextAlignment = TextAlignment.Center
            };

            Image imageToggle = new Image()
            {
                HeightRequest = 30,
                WidthRequest = 30,
                Source = "DownArrow",
                VerticalOptions = LayoutOptions.Center
            };

            //
            stackLayoutHeader.Children.Add(headerLabel);
            stackLayoutHeader.Children.Add(imageToggle);
            expander.Header = stackLayoutHeader;

            expander.HeaderIconPosition = IconPosition.None;
            //Expander Content
            var contentStackLayout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Center
            };

            var contentLabel = new Label()
            {
                Margin = new Thickness(10),
                FontFamily = "STCForwardRegular",
                FontSize =17,
                Opacity = 0.61,
                TextColor = Color.White,
                Text = answer,
                TextType = TextType.Html,
                VerticalOptions = LayoutOptions.Center,
                VerticalTextAlignment = TextAlignment.Center,
            };
            contentStackLayout.Children.Add(contentLabel);
            expander.Content = contentStackLayout;

            Expanders.Children.Add(expander);

            expander.Expanding += (s, a) =>
            {
                try
                {
                    imageToggle.Source = "UpArrow";
                    if (PreExpander != null)
                    {
                        PreExpander.IsExpanded = false;
                        StackLayout _stackLayoutHeader = PreExpander.Header as StackLayout;

                        Image toggle = _stackLayoutHeader.Children[1] as Image;

                        toggle.Source = "DownArrow";
                    }
                    PreExpander = expander;
                }
                catch 
                {

                }
              
            };
            expander.Collapsing += (s, a) =>
            {
                imageToggle.Source = "DownArrow";
            };
        }

        private void AddSeparator()
        {
            BoxView boxView = new BoxView()
            {
                HeightRequest = 1,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.White
            };

            Expanders.Children.Add(boxView);
        }

        private void BorderlessEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string newTextValue = e.NewTextValue.ToLower();

                if (string.IsNullOrEmpty(newTextValue))
                {
                    DrawFAQsList(FAQsSourse);
                    ViewModel.IsNoData = false;
                }
                else
                {
                    bool isAR = ViewModel.Lang == (int)Common.Enums.Languages.Arabic;
                    if (isAR)
                    {
                        IEnumerable<FAQDTO> fillterdFaqs = FAQsSourse.Where(f => f.answerAr.ToLower().Contains(newTextValue) || f.questionAr.ToLower().Contains(newTextValue));

                        DrawFAQsList(fillterdFaqs);

                        ViewModel.IsNoData = fillterdFaqs.Count() == 0;
                    }
                    else
                    {
                        IEnumerable<FAQDTO> fillterdFaqs = FAQsSourse.Where(f => f.answer.ToLower().Contains(newTextValue) || f.question.ToLower().Contains(newTextValue));

                        DrawFAQsList(fillterdFaqs);

                        ViewModel.IsNoData = fillterdFaqs.Count() == 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
