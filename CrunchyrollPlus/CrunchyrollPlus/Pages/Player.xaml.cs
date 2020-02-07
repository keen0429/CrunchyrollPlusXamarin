using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

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

        Timer a;
        public Player(string mediaId,int index, Media[] medias,bool enterFullScreen)
        {
            nextMedia = !enterFullScreen;
            
            InitializeComponent();

            Init(index, mediaId, enterFullScreen);
            this.medias = medias;
            InitSource();
            InitSkip();
        }
        public Player(string mediaId, int index, string collectionId, bool enterFullScreen)
        {

            InitializeComponent();
            
            GetMedias(collectionId);
            Init(index, mediaId, enterFullScreen);
            InitSource();
            

        }

        
        public void Init(int index, string mediaId, bool enterFullScreen)
           
        {
            

            nextMedia = !enterFullScreen;
            this.mediaId = mediaId;
            this.index = index;
            videoPlayer.UpdateStatus += StatusChange;
            Device.StartTimer(TimeSpan.FromMilliseconds(600), () => UpdateTime()); // Need to have lower than 1000 ms because it is not in sync with the video 



        }

        bool UpdateTime()
        {
           
            timePosition.Text = $"{videoPlayer.Position.Minutes.ToString()}:{videoPlayer.Position.Seconds.ToString()}/{videoPlayer.Duration.Minutes}:{videoPlayer.Duration.Seconds}";
            return true;
        }
        
        void StatusChange(object sender, EventArgs e)
        {

            if (!loadingSpinner.IsRunning) return;

            if (!videoPlayer.Status.Equals(VideoStatus.NotReady))
            {
                

                loadingSpinner.IsRunning = false;
                
                

            }
            
        }
        private async void GetMedias(string collectionId)
        {
            CrunchyrollApi.ListMediaResponse res = await crunchyApi.GetMedias(collectionId);
            if (res.success)
            {
                
                medias = res.medias;
                FindIndex();
                
                InitSkip();
            }
            else
            {

                
            }
        }
        private void FindIndex()
        {
            index = Array.FindIndex<Media>(medias, i => i.iD == mediaId);
        }
        private void InitSkip()
        {

            if(index+1<medias.Length) skip.IsVisible = true;

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
                videoPlayer.Position = new TimeSpan(0, 0, res.playhead);
                
                
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
                
            }
            nextMedia = false;
        }
        protected override void OnDisappearing()
        {
            Console.WriteLine("LOG: Dissapearing");

            crunchyApi.LogProgess(mediaId, (int)videoPlayer.Position.TotalSeconds);


            if (!nextMedia)
            {
                Console.WriteLine("nextMedia false");
                DependencyService.Get<IFullscreenService>().ExitFullscreen();
                DependencyService.Get<IDeviceOrientationService>().ForcePortrait();
            }
            else
            {
                Console.WriteLine("NEXT MEDIA TRU");
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

        private void ShowHide(object sender, EventArgs e)
        {

            if (!DependencyService.Get<IFullscreenService>().IsFullscreen())
            {
                DependencyService.Get<IFullscreenService>().EnterFullscreen();
            }

            mediaControls.IsVisible = !mediaControls.IsVisible;
        }
    }
}