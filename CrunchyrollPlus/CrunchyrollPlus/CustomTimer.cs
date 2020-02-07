using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;


namespace CrunchyrollPlus
{
    class CustomTimer
    {

        public bool shouldRun = true;

        public CustomTimer(TimeSpan timeSpan, Func<bool> callback)
        {
            Device.StartTimer(timeSpan, () =>
             {
                 if (!shouldRun) return false;
                 return callback();
                 
             });
        }

    }
}
