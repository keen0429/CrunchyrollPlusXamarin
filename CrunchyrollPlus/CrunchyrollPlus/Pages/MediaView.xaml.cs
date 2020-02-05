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

        /// <summary>
        /// Used for queue
        /// </summary>
        /// <param name="media"></param>
        /// <param name="doubleTap"></param>
        /// <param name="collectionId"></param>
        public MediaView(Media media, bool doubleTap, string collectionId)
        {
            index = -1;
            InitializeComponent();
            this.collectionId = collectionId;

            episodeScreenshot.Source = media.largeImage;

            if (!media.freeAvailable) episodeCount.Text = "Premium only:    ";
            episodeName.Text = media.name;
            episodeCount.Text +="Episode "+ media.episodeNumber;
            this.media = media;
            this.doubleTap = doubleTap;
            if(doubleTap) InitDoubleTap();
            GetDuration();
            Init();

        }
        /// <summary>
        /// Used for ShowPage
        /// </summary>
        /// <param name="media"></param>
        /// <param name="doubleTap"></param>
        /// <param name="medias"></param>
        /// <param name="index"></param>
        public MediaView(Media media, bool doubleTap, Media[] medias, int index)
        {
            this.index = index;
            InitializeComponent();
            this.medias = medias;
            episodeScreenshot.Source = media.largeImage;
            
            if (!media.freeAvailable) episodeCount.Text = "Premium only:        ";
            episodeName.Text = media.name;
            episodeCount.Text += "Episode " + media.episodeNumber;
            this.media = media;
            this.doubleTap = doubleTap;
            if (doubleTap) InitDoubleTap();

            InitProgress();
            Init();
        }

        private void Init()
        {
            int seconds = media.duration % 60;
            int minutes = (media.duration - seconds) / 60;
            episodeLength.Text = $"{minutes.ToString()}:{minutes.ToString()}";
        }

        async void GetDuration()
        {
            media.duration = await crunchy.GetDuration(media.iD);
            InitProgress();
        }
        private void InitProgress()
        {
            double progress = (double)media.playhead / (double)media.duration;
            progressBar.Progress = progress;
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