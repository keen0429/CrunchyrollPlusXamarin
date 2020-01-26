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

[assembly: Xamarin.Forms.Dependency(typeof(CrunchyrollPlus.Droid.OrientationService))]
namespace CrunchyrollPlus.Droid
{
    public class OrientationService : IDeviceOrientationService
    {
        public void ForceLandscape()
        {
            Console.WriteLine("LOG: FORCELANDSCAPE");
            Global.activity.ForceLandscape();
            
        }
        public void ForcePortrait()
        {
            Global.activity.ForcePortrait();
        }
    }
}