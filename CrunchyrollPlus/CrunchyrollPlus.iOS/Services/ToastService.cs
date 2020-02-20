using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(CrunchyrollPlus.iOS.ToastService))]
namespace CrunchyrollPlus.iOS
{
    public class ToastService : IToastService
    {
        const double LONG_DELAY = 3.5;
        const double SHORT_DELAY = 2.0;

        NSTimer alertDelay;
        UIAlertController alert;

        public void ShowToastLong(string message)
        {
            ShowAlert(message, LONG_DELAY);
        }

        public void ShowToastShort(string message)
        {
            ShowAlert(message, SHORT_DELAY);

        }

        private void ShowAlert(string message, double length)
        {
            alertDelay = NSTimer.CreateScheduledTimer(length, (obj) =>
            {
                DismissAlert();
            });
            alert = UIAlertController.Create(null, message, UIAlertControllerStyle.Alert);
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
        }
        void DismissAlert()
        {
            if (alert != null)
            {
                alert.DismissViewController(true, null);
            }
            if (alertDelay != null)
            {
                alertDelay.Dispose();
            }
        }
    }

}