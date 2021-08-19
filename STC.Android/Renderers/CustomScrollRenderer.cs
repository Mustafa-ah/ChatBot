using System;
using Android.Content;
using Android.Runtime;
using Android.Views;
using STC.Common.CommonControlls;
using STC.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(UnscrollableScrollView), typeof(CustomScrollRenderer))]
namespace STC.Droid.Renderers
{
    public class CustomScrollRenderer : ScrollViewRenderer
    {
        public CustomScrollRenderer(Context context ):base(context)
        {

        }
        //https://forums.xamarin.com/discussion/37835/disable-scrollview-scrolling-on-android
        public override bool OnInterceptTouchEvent(MotionEvent ev)
        {
            //Console.WriteLine(ev.Action.ToString());
            if (ev.Action == MotionEventActions.Scroll || ev.Action == MotionEventActions.Move)
            {
                Android.Widget.Toast.MakeText(Android.App.Application.Context, "Tap stoped", Android.Widget.ToastLength.Long).Show();
                return true;
            }
            else
            {
                return base.OnInterceptTouchEvent(ev);
            }
            //if (Element.is)
            //{
            //    return base.OnInterceptTouchEvent(ev);
            //}
            //else
            //{
            //    return false;
            //}
            //return true;
        }

        //public override bool OnTouchEvent(MotionEvent ev)
        //{
        //    //if (Element.IsScrollEnabled)
        //    //{
        //    //    return base.OnTouchEvent(ev);
        //    //}
        //    //else
        //    //{
        //    //    return false;
        //    //}
        //    return false;
        //}

        //public override bool OnStartNestedScroll(Android.Views.View child, Android.Views.View target, int axes, int type)
        //{
        //    return base.OnStartNestedScroll(child, target, axes, type);
        //}

        //public override bool OnStartNestedScroll(Android.Views.View child, Android.Views.View target, [GeneratedEnum] ScrollAxis nestedScrollAxes)
        //{
        //    return false;// base.OnStartNestedScroll(child, target, nestedScrollAxes);
        //}
    }
}
