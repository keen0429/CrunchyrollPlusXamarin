using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SharpCaster;
using SharpCaster.Models.MediaStatus;

namespace CrunchyrollPlus
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChromecastPlayer : ContentPage
    {
        
        ChromecastWrapper wrapper = ChromecastWrapper.GetSingleton();
        Media media;
        Media[] medias;
        Series anime;
        double time = 0;
        CrunchyrollApi crunchyrollApi = CrunchyrollApi.GetSingleton();
        int index;

        bool paused = false;

        public ChromecastPlayer(Media media, Media[] medias,  Series anime, int index)
        {
            InitializeComponent();
            this.index = index;
            this.media = media;
            this.medias = medias;
            this.anime = anime;
            DependencyService.Get<IFullscreenService>().ExitFullscreen();
            DependencyService.Get<IDeviceOrientationService>().ForcePortrait();
            //PlayPause(wrapper,new EventArgs());
            //Device.StartTimer(TimeSpan.FromSeconds(1), TimeTicker);
            //wrapper.ChromecastService.ChromeCastClient.MediaStatusChanged += ChromeCastClient_MediaStatusChanged;
            BindingContext = this;
            slider.Maximum = media.duration;
            Device.StartTimer(TimeSpan.FromSeconds(1), Tick);
            wrapper.ChromecastService.ChromeCastClient.MediaStatusChanged += ChromeCastClient_MediaStatusChanged;

            wrapper.ChromecastService.ChromeCastClient.ChromecastStatusChanged += ChromeCastClient_ChromecastStatusChanged;
            wrapper.mediaLoaded += Wrapper_mediaLoaded;
        }

        private async void Wrapper_mediaLoaded()
        {
            Navigation.InsertPageBefore(new ChromecastPlayer(medias[index + 1], medias, anime, index + 1), this);
            await Navigation.PopAsync();
        }

        private async void ChromeCastClient_ChromecastStatusChanged(object sender, SharpCaster.Models.ChromecastStatus.ChromecastStatus e)
        {

            bool success = await crunchyrollApi.LogProgess(media.iD, (int)wrapper.ChromecastService.ChromeCastClient.MediaStatus.CurrentTime);
        }

        bool Tick()
        {
            if (!paused)
            {
                time += 1;
                slider.Value = time;
                
            }
            return true;
        }
        private void ChromeCastClient_MediaStatusChanged(object sender, MediaStatus e)
        {
            switch (e.PlayerState)
            {
                case PlayerState.Playing:
                    paused = false;
                    break;
                default:
                    paused = true;
                    break;
            }
            time= wrapper.ChromecastService.ChromeCastClient.MediaStatus.CurrentTime;
            slider.Value = time;
        }


        async void PlayPause(object sender, EventArgs e)
        {
            if (wrapper.ChromecastService.ChromeCastClient.MediaStatus.PlayerState == PlayerState.Playing)
            {
                await wrapper.controller.Pause();
                playPause.Source = "play.png";
            }
            else
            {
                await wrapper.controller.Play();
                playPause.Source = "pause.png";
            }
        }
        async void Exit(object sender, EventArgs e)
        {
            await wrapper.controller.StopApplication();
            await Navigation.PopAsync();
        }
        async void CastMenu (object sender, EventArgs e)
        {
            if (await DisplayAlert("Disconnect", $"Would you like to disconnect from {wrapper.ChromecastService.ConnectedChromecast.FriendlyName}?", "Yes", "No"))
            {
                await wrapper.controller.StopApplication();
                media.playhead = (int)time;
                Navigation.InsertPageBefore(new Player(media, medias, true, anime),this);
                await Navigation.PopAsync();
            }
        }
        async void Forward(object sender, EventArgs e)
        {

            slider.Value += 10;
        }
        async void Rewind(object sender, EventArgs e)
        {
            if (slider.Value < 10)
            {
                slider.Value = 0;
            }
            else
            {
                slider.Value -= 10;
            }
            
        }
        async void Skip(object sender, EventArgs e)
        {

            CrunchyrollApi.StreamDataResponse res = await crunchyrollApi.GetStreamData(medias[index+1].iD);
            if (res.success)
            {
               

                await wrapper.LoadMedia(res.url, res.playhead);
                Console.WriteLine("LOG: DONELOADING");
                
            }
            else
            {
                if (res.message == "NoStream") await DisplayAlert("Couldn't get stream", "Media not available, player going to be exited", "OK");
                else await DisplayAlert("Unknown error", "Unknown error occured, player going to be exited", "OK");
                await Navigation.PopAsync();
            }

            
        }

        private void slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (slider.Value != time)
            {
                wrapper.controller.Seek(slider.Value);
                time = slider.Value;
            }
        }
    }
}