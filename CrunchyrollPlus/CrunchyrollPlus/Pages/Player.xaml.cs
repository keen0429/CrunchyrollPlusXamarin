using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FormsVideoLibrary;
using SharpCaster.Services;
using SharpCaster.Controllers;
using SharpCaster.Models.ChromecastStatus;
using SharpCaster.Models.MediaStatus;
using System.Collections.ObjectModel;
using SharpCaster.Models;
using Nito.AsyncEx;

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
        CustomTimer overlayToggle;
        string sourceUrl;
        static readonly ChromecastService ChromecastService = ChromecastService.Current;
        private SharpCasterDemoController _controller;
        ObservableCollection<Chromecast> chromecasts;

        bool chromecastConnected { get;  set; } = false;
        bool notConnected { get; set; } = true;
 
        
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
            notConnected = true;
            chromecastConnected = false;
            BindingContext = this;
            ChromecastService.Current.ChromeCastClient.ConnectedChanged += ChromeCastClient_ConnectedChanged;
            ChromecastService.Current.ChromeCastClient.ApplicationStarted += ChromeCastClient_ApplicationStarted;

            nextMedia = !enterFullScreen;
            this.mediaId = mediaId;
            this.index = index;
            videoPlayer.UpdateStatus += StatusChange;
            Device.StartTimer(TimeSpan.FromMilliseconds(600), UpdateTime); // Need to have lower than 1000 ms because it is not in sync with the video 
            Device.StartTimer(TimeSpan.FromSeconds(10), CheckChromecast);
            


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
            


            CrunchyrollApi.StreamDataResponse res = await crunchyApi.GetStreamData(mediaId);
            if (res.success)
            {
                Console.WriteLine("LOG: SOURCE SUCCESS");
                sourceUrl = res.url;
                videoPlayer.Source = VideoSource.FromUri(res.url);
                videoPlayer.Position = new TimeSpan(0, 0, res.playhead);
                
                
            }
            else
            {
                if (res.message == "NoStream") await DisplayAlert("Couldn't get stream", "Media not available, player going to be exited", "OK");
                else await DisplayAlert("Unknown error", "Unknown error occured, player going to be exited", "OK");
                await Navigation.PopAsync();
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

            if(User.signedIn) crunchyApi.LogProgess(mediaId, (int)videoPlayer.Position.TotalSeconds);



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
                overlayToggle.shouldRun = false;
            }
            else if (videoPlayer.Status == VideoStatus.Paused)
            {
                videoPlayer.Play();
                playPause.Source = "pause.png";
                SetOverlayHideTimer();
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
            Console.WriteLine("Toggle player overlay");
            if (!DependencyService.Get<IFullscreenService>().IsFullscreen())
            {
                DependencyService.Get<IFullscreenService>().EnterFullscreen();
            }

            mediaControls.IsVisible = !mediaControls.IsVisible;

            if (mediaControls.IsVisible)
            {
                SetOverlayHideTimer();
            }
            else
            {
                overlayToggle.shouldRun = false;
            }
            
        }

        private void SetOverlayHideTimer()
        {
            overlayToggle = new CustomTimer(new TimeSpan(0, 0, 5), () =>
            {
                mediaControls.IsVisible = false;
                return false;
            });
        }
        private async void Cast(object sender, EventArgs e)
        {
            //ObservableCollection<Chromecast> chromecasts = await ChromecastService.Current.StartLocatingDevices();

            string[] buttons = chromecasts.Select((i) => i.FriendlyName).ToArray();
            string action = await DisplayActionSheet("Select device to cast to", "Cancel", null, buttons);
            foreach(Chromecast i in chromecasts)
            {
                if (i.FriendlyName == action)
                {
                    await ChromecastService.ConnectToChromecast(i);
                    chromecastConnected = true;
                    notConnected = false;

                }
            }
        }

        private async void ChromeCastClient_ApplicationStarted(object sender, ChromecastApplication e)
        {
            if (_controller == null)
            {
                _controller = await ChromecastService.ChromeCastClient.LaunchSharpCaster();
            }
            
            await _controller.LoadMedia(sourceUrl, "video/mp4", null, "BUFFERED", 0D,null,null,null,true,videoPlayer.Position.TotalSeconds);
            

        }

        private async void ChromeCastClient_ConnectedChanged(object sender, EventArgs e)
        {
            _controller = await ChromecastService.ChromeCastClient.LaunchSharpCaster();

        }
        private void CastMenu(object sender, EventArgs e)
        {

        }
        bool CheckChromecast()
        {
            Task.Run(async () =>
            {
                chromecasts = await ChromecastService.Current.StartLocatingDevices();
                if (chromecasts.Count > 0)
                {
                    EnableChromecast();
                }
                else
                {
                    DisableChromecast();
                }
                
            });
            return true;

        }
        void EnableChromecast()
        {
            castButton.IsVisible = true;
        }
        void DisableChromecast()
        {
            castButton.IsVisible = false;
        }


    }
}