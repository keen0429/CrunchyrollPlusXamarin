using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FormsVideoLibrary;


namespace CrunchyrollPlus
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Player : ContentPage
    {
        CrunchyrollApi crunchyApi = CrunchyrollApi.GetSingleton();
        string mediaId;
        int index;
        Media[] medias;
        bool nextMedia = false;
        public Player(string mediaId,int index, Media[] medias, bool enterFullScreen)
        {
            nextMedia = !enterFullScreen;
            InitializeComponent();
            this.mediaId = mediaId;
            this.index = index;
            this.medias = medias;
            InitSource();
            InitSkip();
        }
        public Player(string mediaId, int index, string collectionId, bool enterFullScreen)
        {
            nextMedia = !enterFullScreen;
            InitializeComponent();
            this.mediaId = mediaId;
            this.index = index;

            InitSource();
            GetMedias(collectionId);

        }
        private async void GetMedias(string collectionId)
        {
            CrunchyrollApi.ListMediaResponse res = await crunchyApi.GetMedias(collectionId);
            if (res.success)
            {
                medias = res.medias;
                InitSkip();
            }
            else
            {

                
            }
        }
        private void InitSkip()
        {
            skip.IsVisible = true;
        }
        async private void InitSource()
        {
            Console.WriteLine("LOG: SOURCE CALL");
            CrunchyrollApi.StreamDataResponse res = await crunchyApi.GetStreamData(mediaId);
            Console.WriteLine("LOG: RES DONE");
            if (res.success)
            {
                Console.WriteLine("LOG: SOURCE SUCCESS");

                videoPlayer.Source = VideoSource.FromUri(res.url);
                videoPlayer.Duration.Add(new TimeSpan(0, 0, res.playhead));
            }
            else
            {
                Console.WriteLine("LOG: SOURCE ERROR :   " + res.message);
            }
            
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Console.WriteLine("LOG: METHOD RAN");
            
            if(!nextMedia)
            {
                DependencyService.Get<IFullscreenService>().EnterFullscreen();
                IDeviceOrientationService service = DependencyService.Get<IDeviceOrientationService>();
                service.ForceLandscape();
                nextMedia = false;
            }
        }
        protected override void OnDisappearing()
        {
            Console.WriteLine("LOG: Dissapearing");
            if (!nextMedia)
            {
                DependencyService.Get<IFullscreenService>().ExitFullscreen();
                DependencyService.Get<IDeviceOrientationService>().ForcePortrait();
            }
            base.OnDisappearing();
            
            

        }

        private void playPause_Clicked(object sender, EventArgs e)
        {
            if (videoPlayer.Status == VideoStatus.Playing)
            {
                videoPlayer.Pause();
                playPause.Source = "play.png";
            }
            else if (videoPlayer.Status == VideoStatus.Paused)
            {
                videoPlayer.Play();
                playPause.Source = "pause.png";
            }
            else
            {

            }
            
        }

        private void rewind_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("LOG: REWIND");

            videoPlayer.Position = videoPlayer.Position.Subtract(new TimeSpan(0, 0, 10));
        }

        private void forward_Clicked(object sender, EventArgs e)
        {
            videoPlayer.Position = videoPlayer.Position.Add(new TimeSpan(0, 0, 10));

        }


        async private void back_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void skip_Clicked(object sender, EventArgs e)
        {
            nextMedia = true;
            Navigation.InsertPageBefore(new Player(medias[index + 1].iD, index + 1, medias,false),this);
            await Navigation.PopAsync();
        }
    }
}