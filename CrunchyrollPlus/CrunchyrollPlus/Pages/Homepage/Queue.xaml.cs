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
        public Media[] medias { get; private set; }
        private CrunchyrollApi.QueueEntry[] entries;
        public Queue()
        {
            InitializeComponent();
            InitQueue();
        }

        private async void InitQueue()
        {
            CrunchyrollApi.GetQueueResponse res = await crunchyApi.GetQueue();
            //queueMedia.Children.Clear();
            if (res.success)
            {


                medias = res.entry.Select(e => e.mostLikely).ToArray();
                entries = res.entry;
                BindingContext = this;
                mediaList.IsRefreshing = false;
            }
            else
            {
            }

        }
        private void Refresh(object sender, EventArgs e)
        {
            InitQueue();
            
        }
        private void AddMedia(Media media, int index)
        {

            //queueMedia.Children.Add(new MediaView(media, true, media.collectionId));

        }
        private async void OpenMedia(object sender, ItemTappedEventArgs e)
        {
            Media selectedMedia = (Media)mediaList.SelectedItem;
            

            await Navigation.PushAsync(new Player(selectedMedia, true,entries[Array.FindIndex<Media>(medias, i => i.iD==selectedMedia.iD)].series));
        }

        private async  void OpenShow(object sender, EventArgs e)
        {
            Series selectedSeries = entries[0].series; // Not going to be used but just in case so the compiler doesn't complain
            for(int i = 0; i < medias.Length; i++)
            {
                if ((string)((MenuItem)sender).CommandParameter == entries[i].mostLikely.iD)
                {
                    selectedSeries = entries[i].series;
                }
            }
            await Navigation.PushAsync(new ShowPage(selectedSeries));
        }
    }
}