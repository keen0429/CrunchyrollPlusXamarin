using System;

using System.Collections.Generic;
using System.Text;
using SharpCaster.Services;
using SharpCaster.Controllers;
using SharpCaster.Models.ChromecastStatus;
using SharpCaster.Models.MediaStatus;
using System.Collections.ObjectModel;
using SharpCaster.Models;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Linq;

namespace CrunchyrollPlus
{
    public class ChromecastWrapper
    {

        bool applicationStarted = false;
        string connectToUrl = "";
        double timeStamp = 0;


        private static readonly ChromecastWrapper singleton = new ChromecastWrapper();

        public static ChromecastWrapper GetSingleton()
        {
            return singleton;
        }
        public SharpCasterDemoController controller;
        public ObservableCollection<Chromecast> chromecasts = new ObservableCollection<Chromecast>();
        public readonly ChromecastService ChromecastService = ChromecastService.Current;



        public event ChromecastPresentChangeHandler chromecastChange;
        public delegate void ChromecastPresentChangeHandler(bool present);
        
        private ChromecastWrapper()
        {
            Device.StartTimer(TimeSpan.FromSeconds(10), CheckChromecast);
            ChromecastService.Current.ChromeCastClient.ConnectedChanged += ChromeCastClient_ConnectedChanged;
            ChromecastService.Current.ChromeCastClient.ApplicationStarted += ChromeCastClient_ApplicationStarted;
            
        }
        bool CheckChromecast()
        {
            Task.Run(async () =>
            {
                
                //chromecasts = await ChromecastService.Current.DeviceLocator.LocateDevicesAsync();
                chromecasts = await ChromecastService.StartLocatingDevices();
                //chromecasts = await ChromecastService.DeviceLocator.LocateDevicesAsync();
                
                Console.WriteLine("LOG   checkCompleted");
                chromecastChange(chromecasts.Count > 0);


            });
            return true;

        }
        public Chromecast SelectChromecast(string action)
        {
            foreach (Chromecast i in chromecasts)
            {
                if (i.FriendlyName == action)
                {
                    return i;

                }
            }

            return null; // This shappens if user doesn't select any device

        }

        public string[] GetChromecastOptions()
        {
            return chromecasts.Select((i) => i.FriendlyName).Distinct().ToArray();
        }
        private async void ChromeCastClient_ApplicationStarted(object sender, ChromecastApplication e)
        {
            if (controller == null)
            {
                   
                controller = await ChromecastService.ChromeCastClient.LaunchSharpCaster();
            }
            applicationStarted = true;
            if (connectToUrl != "")
            {
                await controller.LoadMedia(connectToUrl, "video/mp4", null, "BUFFERED", 0D, null, null, null, true, timeStamp);
            }

            


        }

        private async void ChromeCastClient_ConnectedChanged(object sender, EventArgs e)
        {
            controller = await ChromecastService.ChromeCastClient.LaunchSharpCaster();

        }
        public Task LoadMedia(string url, double currentTime = 0)
        {
            if (applicationStarted) return controller.LoadMedia(url, "video/mp4", null, "BUFFERED", 0D, null, null, null, true, currentTime);
            
            else
            {
                connectToUrl = url;
                timeStamp = currentTime;
                return Task.Run(() => { });
            }


        }



    }
}
