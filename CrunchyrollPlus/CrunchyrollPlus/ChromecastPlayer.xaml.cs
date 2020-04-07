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

        

        public ChromecastPlayer(Media media, Media[] medias,  Series anime)
        {
            InitializeComponent();
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
            
            
        }

        bool Tick()
        {
            time += 1;
            slider.Value = time;
            return true;
        }
        private void ChromeCastClient_MediaStatusChanged(object sender, MediaStatus e)
        {
            
            time= wrapper.ChromecastService.ChromeCastClient.MediaStatus.CurrentTime;
            slider.Value = time;
        }


        async void PlayPause(object sender, EventArgs e)
        {
            if (wrapper.ChromecastService.ChromeCastClient.MediaStatus.PlayerState == PlayerState.Playing)
            {
                await wrapper.controller.Pause();
            }
            else
            {
                await wrapper.controller.Play();
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