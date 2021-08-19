using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace STC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FAQ : ContentPage
    {
        public FAQ()
        {
            InitializeComponent();
            var faqs = new List<Models.FAQ>();


            faqs.Add(new Models.FAQ()
            {
                Question = "What Papers Do you need ?",
                Answer = "Lorem Ipsum is simply dummy text of the printing and typesetting industry",
                isVis = true

            });
            faqs.Add(new Models.FAQ()
            {
                Question = "What papers do you need?",
                Answer = "Lorem Ipsum is simply dummy text of the printing and typesetting industry",
                isVis = false

            });
            faqs.Add(new Models.FAQ()
            {
                Question = "What papers do you need?",
                Answer = "Lorem Ipsum is simply dummy text of the printing and typesetting industry",
                isVis = false

            });
            faqs.Add(new Models.FAQ()
            {
                Question = "What Papers Do you need ?",
                Answer = "Lorem Ipsum is simply dummy text of the printing and typesetting industry",
                isVis = true

            });

            list.ItemsSource = faqs;
        }
    }
}