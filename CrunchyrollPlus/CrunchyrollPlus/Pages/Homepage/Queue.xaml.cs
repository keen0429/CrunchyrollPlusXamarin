using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CrunchyrollPlus
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Queue : ContentPage
    {
        CrunchyrollApi crunchyApi = CrunchyrollApi.GetSingleton();
        public Queue()
        {
            InitializeComponent();
        }

        protected override void OnAppearing() // I want to refresh every time the user enter queue
        {

            InitQueue();
            Console.WriteLine("LOG: QUEUE APPEARING ");
        }
        private async void InitQueue()
        {

            CrunchyrollApi.GetQueueResponse res = await crunchyApi.GetQueue();
            queueMedia.Children.Clear();
            if (res.success)
            {
                for (int i = 0; i < res.entry.Length; i++)
                {
                    AddMedia(res.entry[i].mostLikely, -1);
                }

            }
            else
            {
            }
        }

        private void AddMedia(Media media, int index)
        {

            queueMedia.Children.Add(new MediaView(media, true, media.collectionId));

        }
    }
}