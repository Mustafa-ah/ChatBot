using System;
using Xamarin.Forms;

namespace STC.Common.CommonControlls
{
    public class BorderlessEntry : Entry
    {
        public bool IsOTP { get; set; }

        public event EventHandler Delete;
        public event EventHandler<string> EditingChanged;

        public void OnDelete()
        {
            Delete?.Invoke(this, new EventArgs());
        }

        public void OnEditingChanged(string newText)
        {
            EditingChanged?.Invoke(this, newText);
        }

    }

}
