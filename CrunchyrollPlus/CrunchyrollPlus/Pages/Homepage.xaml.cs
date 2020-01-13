using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json.Linq;
using JikanDotNet;

namespace CrunchyrollPlus
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Homepage : TabbedPage
    {
        int column = 0;
        int row = 0;
        CrunchyrollApi crunchyApi = CrunchyrollApi.GetSingleton();
        public Homepage()
        {
            
            InitializeComponent();
            InitQueue();
        }

        private async void InitQueue()
        {
            Debug.WriteLine("LOG: INIT QUEUE");

            CrunchyrollApi.GetQueueResponse res = await crunchyApi.GetQueue();
            if (res.success)
            {
                foreach(CrunchyrollApi.QueueEntry i in res.entry)
                {
                    Debug.WriteLine("LOG: ITTERATION");

                    AddMedia(i.mostLikely);
                }
            }
            else
            {
                Debug.WriteLine("LOG: ERROR: " + res.message);
            }
        }

        private void AddMedia(Media media)
        {
            
            Debug.WriteLine("LOG: DD");
            queueMedia.Children.Add(new MediaView(media));

        }
        private void InsertMALWatching()
        {
            
        }
    }
}