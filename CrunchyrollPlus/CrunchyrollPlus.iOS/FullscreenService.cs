using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(CrunchyrollPlus.iOS.FullscreenService))]
namespace CrunchyrollPlus.iOS
{
    
    class FullscreenService : IFullscreenService
    {
        public void EnterFullscreen()
        {
            UIApplication.SharedApplication.StatusBarHidden = true;
        }

        public void ExitFullscreen()
        {
            UIApplication.SharedApplication.StatusBarHidden = false;

        }
        public bool IsFullscreen()
        {
            return UIApplication.SharedApplication.StatusBarHidden;
        }
    }
}