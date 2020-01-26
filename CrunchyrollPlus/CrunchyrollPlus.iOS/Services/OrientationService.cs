using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(CrunchyrollPlus.iOS.OrientationService))]
namespace CrunchyrollPlus.iOS
{
    public class OrientationService : IDeviceOrientationService
    {
        public void ForceLandscape()
        {
            UIDevice.CurrentDevice.SetValueForKey(new NSNumber((int)UIInterfaceOrientation.LandscapeRight), new NSString("orientation"));
        }
        public void ForcePortrait()
        {
            UIDevice.CurrentDevice.SetValueForKey(new NSNumber((int)UIInterfaceOrientation.Portrait), new NSString("orientation"));

        }
    }
}