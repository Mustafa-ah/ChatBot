using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace STC.Common.CommonControlls
{
    public partial class OTPContentView : ContentView
    {
        public OTPContentView()
        {
            InitializeComponent();
  
        }
        public static readonly BindableProperty LangProperty =
            BindableProperty.Create(nameof(Lang), typeof(int), typeof(OTPContentView), (int)Enums.Languages.English,
                BindingMode.TwoWay);




        public int Lang
        {
            get { return (int)GetValue(LangProperty); }
            set { SetValue(LangProperty, value); }
        }
        public static readonly BindableProperty OTPDigitProperty = BindableProperty.Create(nameof(OTPDigit), typeof(string), typeof(OTPContentView), null, BindingMode.TwoWay);

        public string OTPDigit
        {
            get => (string)this.GetValue(OTPDigitProperty);
            set => this.SetValue(OTPDigitProperty, value);
        }

        public static readonly BindableProperty EntryReturnCommandProperty =
          BindableProperty.Create(nameof(EntryReturnCommand), typeof(ICommand), typeof(OTPContentView), null);

        public ICommand EntryReturnCommand
        {
            get { return (ICommand)GetValue(EntryReturnCommandProperty); }
            set { SetValue(EntryReturnCommandProperty, value); }
        }
        public static void Execute(ICommand command)
        {
            if (command == null) return;
            if (command.CanExecute(null))
            {
                command.Execute(null);
            }
        }

        public OTPContentView NextEntry { get; set; }
        public OTPContentView PreviousEntry { get; set; }

        void EntryControll_Delete(System.Object sender, System.EventArgs e)
        {
            if (PreviousEntry != null)
            {
                PreviousEntry.EntryControll.Focus();
                PreviousEntry.EntryControll.CursorPosition = 0;
                PreviousEntry.EntryControll.SelectionLength = 1;
            }

            /*if (PreviousEntry != null && Lang == 2) // en, back to left when delete
            {
                PreviousEntry.EntryControll.Focus();
                PreviousEntry.EntryControll.CursorPosition = 0;
                PreviousEntry.EntryControll.SelectionLength = 1;
            }
            if (NextEntry != null && Lang == 1) // ar, back to right when delete
            {
                NextEntry.EntryControll.Focus();
                NextEntry.EntryControll.CursorPosition = 0;
                NextEntry.EntryControll.SelectionLength = 1;
            }*/
        }

        void EntryControll_EditingChanged(System.Object sender, string NewTextValue)
        {

         
            if (OTPDigit == ".")
            {
                OTPDigit = "";
                return;

            }
            var next = NextEntry;
            int count_next = 0;
            int count_prev = 0;
            int count_total = 0;
            if (!string.IsNullOrEmpty(NewTextValue))
                count_total+=1;
            while (next != null)
            {
                if (!string.IsNullOrEmpty(next.GetValue(OTPDigitProperty) as string))
                    count_next++;
                else
                    break;
                next = next.NextEntry;

            }
            var prev = PreviousEntry;
            while (prev != null)
            {
                if (!string.IsNullOrEmpty(prev.GetValue(OTPDigitProperty) as string))
                    count_prev++;
                else
                    break;
                prev = prev.PreviousEntry;
            }
            count_total += count_prev + count_next;
            if(count_total==4)
            {
                BorderlessEntry current = (BorderlessEntry)sender;
                current.Unfocus();
            }
            else if (NextEntry != null && NewTextValue.Length == 1)
            {
                NextEntry.EntryControll.Focus();
            }
            /*else if (NextEntry != null && NewTextValue.Length == 1 && Lang==2) // en, forword from left to right
            {
                NextEntry.EntryControll.Focus();
            }
            else if (PreviousEntry != null && NewTextValue.Length == 1 && Lang==1) // ar, forword from right to left
            {
                PreviousEntry.EntryControll.Focus();
            }*/
            //else if(NextEntry ==null)
            //{
            //    BorderlessEntry current = (BorderlessEntry)sender;
            //    current.Unfocus();
            //}


            Execute(EntryReturnCommand);
        }

        public void RequestFocus()
        {
            EntryControll.Focus();
        }
    }
}
