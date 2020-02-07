using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

[assembly: Xamarin.Forms.Dependency(typeof(CrunchyrollPlus.Droid.ToastService))]
namespace CrunchyrollPlus.Droid
{
    
    public class ToastService : IToastService
    {
        public void ShowToastShort(string message)
        {
            
            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }
        public void ShowToastLong(string message)
        {
            

            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }

    }
}