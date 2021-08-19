using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using STC.Common.CommonControlls;
using STC.Droid.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ChatEditor), typeof(ChatEditorRenderer))]
namespace STC.Droid.Renderers
{
    public class ChatEditorRenderer : EditorRenderer
    {
        public ChatEditorRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                GradientDrawable gd = new GradientDrawable();
                gd.SetColor(global::Android.Graphics.Color.Transparent);
                this.Control.SetBackground(gd);
                this.Control.SetRawInputType(InputTypes.TextFlagNoSuggestions);
                Control.SetHintTextColor(ColorStateList.ValueOf(global::Android.Graphics.Color.Black));
            }

            if (e.OldElement != null)
            {
                Control.EditorAction -= Control_EditorAction;
            }

            if (e.NewElement != null)
            {
                Control.EditorAction += Control_EditorAction;
            }
        }

        private void Control_EditorAction(object sender, TextView.EditorActionEventArgs e)
        {
            Control.Text += "\n";
            Control.SetSelection(Control.Text.Length - 1);
        }

    }
}