using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;

namespace CrunchyrollPlus
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MediaView : ContentView
    {
        Media media;
        CrunchyrollApi crunchy = CrunchyrollApi.GetSingleton();
        Series series;
        bool doubleTap;
        Media[] medias;
        string collectionId="";
        int index;

        public MediaView(Media media, bool doubleTap, string collectionId)
        {
            
            InitializeComponent();
            this.collectionId = collectionId;

            if (media.freeAvailable) episodeScreenshot.Source = media.largeImage;
            else episodeScreenshot.Source = media.largeImageStar;
            episodeName.Text = media.name;
            episodeCount.Text ="Episode "+ media.episodeNumber;
            this.media = media;
            this.doubleTap = doubleTap;
            if(doubleTap) InitDoubleTap();


        }
        public MediaView(Media media, bool doubleTap, Media[] medias)
        {
            InitializeComponent();
            this.medias = medias;
            if (media.freeAvailable) episodeScreenshot.Source = media.largeImage;
            else episodeScreenshot.Source = media.largeImageStar;
            episodeName.Text = media.name;
            episodeCount.Text = "Episode " + media.episodeNumber;
            this.media = media;
            this.doubleTap = doubleTap;
            if (doubleTap) InitDoubleTap();
        }
        
        private async void InitDoubleTap()
        {
            CrunchyrollApi.GetSeriesResponse res = await crunchy.GetSeries(media.seriesId);
            if (res.success)
            {
                Debug.WriteLine("LOG: SUCCESS SHOW");
                series = res.series;
            }
            else
            {
                Debug.WriteLine("LOG: FAILED SHOW");
                Debug.WriteLine("LOG: FAIL: " + res.message);

                //Error handeling;
            }
        }
        private async void OnOpenShow(object sender, EventArgs e)
        {
            if (doubleTap)
            {
                Debug.WriteLine("LOG: SER: " + series.fullImagePortrait);
                await Navigation.PushAsync(new ShowPage(series));
            }
            
        }
        private async void OnOpenMedia(object sender, EventArgs e)
        {
            if (!media.freeAvailable) return;
            if(collectionId=="") await Navigation.PushAsync(new Player(media.iD, index, medias,true));
            else await Navigation.PushAsync(new Player(media.iD,index,collectionId,true));
        }
    }
}