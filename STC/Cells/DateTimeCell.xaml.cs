using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace STC.Cells
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DateTimeCell : ViewCell
    {
        public DateTimeCell()
        {
            InitializeComponent();
        }
    }
}
