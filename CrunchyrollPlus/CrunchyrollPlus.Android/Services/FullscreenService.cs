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

[assembly: Xamarin.Forms.Dependency(typeof(CrunchyrollPlus.Droid.FullscreenService))]
namespace CrunchyrollPlus.Droid
{
    public class FullscreenService : IFullscreenService
    {
        public void EnterFullscreen()
        {
            

            int uiOptions = (int)Global.activity.Window.DecorView.SystemUiVisibility;
           uiOptions|= (int)SystemUiFlags.Fullscreen;
            uiOptions |= (int)SystemUiFlags.Immersive;
            uiOptions |= (int)SystemUiFlags.HideNavigation;


            Global.activity.Window.DecorView.SystemUiVisibility = (StatusBarVisibility)uiOptions;
        }

        public void ExitFullscreen()
        {
            Global.activity.Window.AddFlags(WindowManagerFlags.ForceNotFullscreen);
        }
    }
}